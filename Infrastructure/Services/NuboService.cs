using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class NuboService : INuboService
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;
        private readonly Uri _baseUri = new Uri("https://api-test.nubowms.pl/api/v1/");
        public NuboService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _token = config["Nubo:Token"];  // placeholder, potem jak dodam token to ewentualnie zaktualizować nazwę.
        }
        public async Task<string> CheckIfProductsAreInNubo(List<string> productsSkus, CancellationToken cancellationToken)
        {
            string skusAsString = string.Join(",", productsSkus);

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(_baseUri, $"products?productCodes={skusAsString}"));

            request.Headers.Add("Authorization", $"Bearer {_token}");

            var response = _httpClient.SendAsync(request, cancellationToken);

           response.Result.EnsureSuccessStatusCode();

            JsonDocument doc = JsonDocument.Parse(await response.Result.Content.ReadAsStringAsync(cancellationToken));

            string products = doc.RootElement.GetProperty("items").ToString();

            return products;
        }
    }
}
