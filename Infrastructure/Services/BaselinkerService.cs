using Application.Interfaces;
using Application.Models.DTOs;
using System.Text.Json;

namespace Infrastructure.Services
{
    internal class BaselinkerService : IBaselinkerService
    {
        private readonly HttpClient _httpClient;
        public BaselinkerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
            
            request.Headers.Add("X-BLToken", "tu dać token z baselinkera");
            
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
                invnetory_id = "tu dać inv id z bla",
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
            request.Headers.Add("X-BLToken", "tu dać token z baselinkera");

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

        public async Task<int> SendProductToBaselinker(ProductDTO product, CancellationToken cancellationToken)
        {
            var payload = new
            {

                inventory_id = "tu dać inv id z bla",
                ean = product.Ean,
                sku = product.Sku,
                manufacturer_id = product.Brand.BaselinkerId,
                category_id = product.Category.BaselinkerId,
                prices = "tutaj trzeba dac dictionary z ceną przynajmniej w pln",
                stock = "dodać do produktu quantity i przekazywać tu",
                text_fields = "pola tekstowe, tutaj będzie nazwa opis i chyba parametry ale jeszcze sprawdzić"
            };

            var apiParams = new Dictionary<string, string>
            {
                { "method", "addInventoryItem" },
                { "parameters", JsonSerializer.Serialize(payload) }
            };

            var content = new FormUrlEncodedContent(apiParams);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.baselinker.com/connector.php")
            {
                Content = content
            };

            request.Headers.Add("X-BLToken", "tu dać token z baselinkera");

            var response = await _httpClient.SendAsync(request, cancellationToken);
            string result = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode && result.Contains("SUCCESS"))
            {
                JsonDocument doc = JsonDocument.Parse(result);
                int productId = doc.RootElement.GetProperty("product_id").GetInt32();
                return productId;
            }

            return 0;
        }
    }
}
