using Application.Interfaces;
using Application.Models.DTO_s;
using Application.Models.Queries.SecretQueries;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Net.Sockets;
using System.Text.Json;

namespace Infrastructure.Services
{
    internal class BaselinkerService : IBaselinkerService
    {
        private readonly HttpClient _httpClient;
        private string? _token;
        private readonly IMediator _mediator;
        public BaselinkerService(HttpClient httpClient, IConfiguration config, IMediator mediator)
        {
            _httpClient = httpClient;
            _mediator = mediator;
            _token = config["Baselinker:Token"];
        }

        private async Task<string> GetSecretAsync(string secretName, CancellationToken cancellationToken)
        {
            string secret = string.Empty;

            if (string.IsNullOrEmpty(_token))
            {
                secret = await _mediator.Send(new GetSecretQuery(secretName), cancellationToken);
            }

            return secret;
        }
        public async Task<string> GetBrands(CancellationToken cancellationToken)
        {
            _token = await GetSecretAsync("token", cancellationToken);

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
            _token = await GetSecretAsync("token", cancellationToken);
            string inwentoryId = await GetSecretAsync("catalogueId", cancellationToken);

            var parameters = new
            {
                inventory_id = inwentoryId, //id kat głowny
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
            _token = await GetSecretAsync("token", cancellationToken);
            string inventoryId = await GetSecretAsync("catalogueId", cancellationToken);
            string warehouseId = await GetSecretAsync("warehouseId", cancellationToken);

            var payload = new
            {

                inventory_id = inventoryId, //id kat głowny
                parent_id = product.ParentId,
                ean = product.Ean,
                sku = product.Sku,
                manufacturer_id = product.BrandId,
                category_id = product.CategoryId,
                prices = product.Prices,
                stock = new Dictionary<string, int>
                {
                    { warehouseId, 0 }, //id magazynu głównego,
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
