using Application.Interfaces;
using Application.Models.DTO_s;
using Application.Models.DTOs;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.Services
{
    public class ProductPreparationService : IProductPreparationService
    {
        public string GenerateDescription(ProductDTO product, MappingDTO mapping)
        {
            StringBuilder description = new StringBuilder();
            description.AppendLine($"<h2>{GenerateTitle(product, mapping)}</h2>");
            description.AppendLine();
            description.AppendLine($"<p>{mapping.Description}</p>");
            description.AppendLine();
            description.AppendLine("<h3>Parametry produktu</h3>");

            foreach (var parameter in product.Parameters)
            {
                if (string.IsNullOrWhiteSpace(parameter.Name) || string.IsNullOrWhiteSpace(parameter.Value))
                    continue;
                if (parameter.Name == "Sku bez koloru" ||
                    parameter.Name == "HS Code" ||
                    parameter.Name == "Kraj pochodzenia" || 
                    parameter.Name == "Oznaczenie płci")
                    continue;

                description.AppendLine($"<p>* <b>{parameter.Name}</b>: {parameter.Value}</p>");
            }

            return description.ToString();
        }

        public string GenerateParametersString(ProductDTO product)
        {
            return string.Join(';', product.Parameters.Select(p => $"{p.Name}: {p.Value}"));
        }

        public string GenerateTitle(ProductDTO product, MappingDTO mapping)
        {
            return $"{product.Category.BaselinkerName} " +
                $"{product.Brand.Name} " +
                $"{product.Parameters?.FirstOrDefault(p => p.Name.ToLower().Contains("rozmiar"))?.Value} " +
                $"{product.Parameters?.FirstOrDefault(p => p.Name.ToLower().Contains("kolor"))?.Value}";
            
        }

        public ProductToBaselinkerDTO PrepareProduct(ProductDTO product, MappingDTO mapping)
        {
            string title = GenerateTitle(product, mapping);
            string description = GenerateDescription(product, mapping);
            string descriptionWithoutHtml = Regex.Replace(description, "<.*?>", string.Empty);
            string parametersString = GenerateParametersString(product);

            Dictionary<string, string> parametersDictionary = new Dictionary<string, string>();

                foreach (var parameter in product.Parameters)
                {
                    if (string.IsNullOrWhiteSpace(parameter.Name) || string.IsNullOrWhiteSpace(parameter.Value))
                        continue;

                    if (parameter.Name.ToLower() == "płeć" && !product.Parameters.Any(p => p.Name == "Oznaczenie płci"))
                    {
                        switch (parameter.Value.ToLower())
                        {
                            case "męskie":
                                parametersDictionary.Add("Oznaczenie płci", "M");
                                break;
                            case "damskie":
                                parametersDictionary.Add("Oznaczenie płci", "F");

                                break;
                            case "unisex":
                                parameter.Value = "U";
                                break;
                            default:
                                break;
                        }
                    }

                    parametersDictionary.Add(parameter.Name, parameter.Value);
                }

            if (product.BaselinkerParentId == 0)
                product.Ean = "";

            return new ProductToBaselinkerDTO
            {
                Id = product.BaselinkerId,
                ParentId = product.BaselinkerParentId,
                Prices = new Dictionary<int, decimal>
                {
                    { 3276, product.Price }
                },
                CategoryId = product.Category.BaselinkerId,
                BrandId = mapping.Brand.BaselinkerId,
                Sku = product.Sku,
                Ean = product.Ean,
                TextFields = new Dictionary<string, object?>
                {
                    { "name", title },
                    { "description", description },
                    { "features", parametersDictionary },
                    { "description_extra1", product.Brand.Description },
                    { "description_extra2", descriptionWithoutHtml },
                    { "extra_field_7262", ExtractMainSku(product.Sku) }, //main sku miinto
                    { "extra_field_7665", (int)(product.Price * 100) }, //cena miinto
                    { "extra_field_7524", parametersDictionary.FirstOrDefault(p => p.Key.ToLower() == "sku bez koloru").Value ?? "" }, // sku miinto bez koloru
                    { "extra_field_7423", parametersDictionary.FirstOrDefault(p => p.Key.ToLower() == "hs code").Value ?? "" }, //hs code
                    { "extra_field_7424", parametersDictionary.FirstOrDefault(p => p.Key.ToLower() == "kraj pochodzenia").Value ?? "" }, // kraj pochodzenia
                    { "extra_field_7525", product.Name ?? "" }, // nazwa miinto
                    { "extra_field_7526", parametersDictionary.FirstOrDefault(p => p.Key.ToLower() == "kolor").Value ?? "" }, // kolor miinto
                    { "extra_field_7527", parametersDictionary.FirstOrDefault(p => p.Key.ToLower() == "rozmiar").Value ?? "" }, // rozmiar miinto
                    { "extra_field_7528", parametersDictionary.FirstOrDefault(p => p.Key.ToLower() == "oznaczenie płci").Value ?? "" } // płeć
                }
            };
        }
        public string ExtractMainSku(string sku) 
        {
            if (sku.Contains("R.") && !sku.Contains("_OS"))
                return sku.Split("R.")[0].Trim();
            if (sku.Contains("_") && !sku.Contains("_OS"))
            {
                int lastUnderscoreIndex = sku.LastIndexOf('_');
                return sku.Substring(0, lastUnderscoreIndex).Trim();
            }

            return sku;
        }
    }
}
