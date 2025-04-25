using Application.Interfaces;
using Application.Models.DTO_s;
using Application.Models.DTOs;
using System.Text;

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
            return $"{product.Category.Name} " +
                $"{product.Brand.Name} " +
                $"{product.Name} " +
                $"{product.Parameters?.FirstOrDefault(p => p.Name.ToLower().Contains("rozmiar"))?.Value}" +
                $"{product.Parameters?.FirstOrDefault(p => p.Name.ToLower().Contains("kolor"))?.Value}";
            
        }

        public ProductToBaselinkerDTO PrepareProduct(ProductDTO product, MappingDTO mapping)
        {
            string title = GenerateTitle(product, mapping);
            string description = GenerateDescription(product, mapping);
            string parametersString = GenerateParametersString(product);

            return new ProductToBaselinkerDTO
            {
                Id = product.BaselinkerId,
                ParentId = product.BaselinkerParentId,
                Prices = new Dictionary<int, decimal>
                {
                    { 3276, product.Price }
                },
                CategoryId = mapping.Category.BaselinkerId,
                BrandId = mapping.Brand.BaselinkerId,
                Sku = product.Sku,
                Ean = product.Ean,
                TextFields = new Dictionary<string, string>
                {
                    { "title", title },
                    { "description", description },
                    { "features", parametersString },
                    {"description_extra1", product.Brand.Description }
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
