using Application.Interfaces;
using Application.Models.DTO_s;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Infrastructure.Services
{
    internal class BaselinkerService : IBaselinkerService
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;
        public BaselinkerService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _token = config["Baselinker:Token"];
        }
        public async Task<string> GetBrands(CancellationToken cancellationToken)
        {
            var apiParams = new Dictionary<string, string>
            {
                { "method", "getInventoryManufacturers" },
                { "parameters", "{}" }
            };

            var content = new FormUrlEncodedContent(apiParams);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.baselinker.com/connector.php")
            {
                Content = content
            };

            request.Headers.Add("X-BLToken", _token);

            var response = _httpClient.SendAsync(request, cancellationToken);

            string result = await response.Result.Content.ReadAsStringAsync(cancellationToken);

            if (response.Result.IsSuccessStatusCode && result.Contains("SUCCESS"))
            {
                JsonDocument doc = JsonDocument.Parse(result);
                string brands = doc.RootElement.GetProperty("manufacturers").ToString();
                return brands;
            }

            return "Error occured while tried to download brands";
        }

        public async Task<string> GetCategories(CancellationToken cancellationToken)
        {
            var parameters = new
            {
                inventory_id = 10621, //id kat głowny
            };

            var apiParams = new Dictionary<string, string>
            {
                { "method", "getInventoryCategories" },
                { "parameters", JsonSerializer.Serialize(parameters) }
            };

            var content = new FormUrlEncodedContent(apiParams);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.baselinker.com/connector.php")
            {
                Content = content
            };
            request.Headers.Add("X-BLToken", _token);

            var response = _httpClient.SendAsync(request, cancellationToken);
            string result = await response.Result.Content.ReadAsStringAsync(cancellationToken);

            if (response.Result.IsSuccessStatusCode && result.Contains("SUCCESS"))
            {
                JsonDocument doc = JsonDocument.Parse(result);
                string categories = doc.RootElement.GetProperty("categories").ToString();
                return categories;
            }

            return "Error occured while tried to download categories";
        }

        public async Task<int> SendProductToBaselinker(ProductToBaselinkerDTO product, CancellationToken cancellationToken)
        {
            var payload = new
            {

                inventory_id = 10621, //id kat głowny
                parent_id = product.ParentId,
                ean = product.Ean,
                sku = product.Sku,
                manufacturer_id = product.BrandId,
                category_id = product.CategoryId,
                prices = product.Prices,
                stock = new Dictionary<string, int>
                {
                    { "bl_5248", 0 }, //id magazynu głównego,
                },
                text_fields = product.TextFields
            };

            var apiParams = new Dictionary<string, string>
            {
                { "method", "addInventoryProduct" },
                { "parameters", JsonSerializer.Serialize(payload) }
            };

            var content = new FormUrlEncodedContent(apiParams);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.baselinker.com/connector.php")
            {
                Content = content
            };

            request.Headers.Add("X-BLToken", _token);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            string result = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode && result.Contains("SUCCESS"))
            {
                JsonDocument doc = JsonDocument.Parse(result);
                int productId = doc.RootElement.GetProperty("product_id").GetInt32();
                return productId;
            }
            else
            {
                var status = (int)response.StatusCode;
                throw new HttpRequestException(
                    $"Request failed. Status code: {status} ({response.StatusCode}). Response content: {result}"
                );
            }
        }
    }
}
