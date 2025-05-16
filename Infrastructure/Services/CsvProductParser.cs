using Application.Interfaces;
using Application.Models.Commands.ProductsCommands;
using Application.Models.DTO_s;
using Application.Models.DTOs;
using CsvHelper;
using System.Globalization;
using System.Reflection;

namespace Infrastructure.Services
{
    public class CsvProductParser : ICsvProductParser
    {
        public async Task<List<ProductDTO>> ParseCsv(CsvImportRequest import)
        {
            List<ProductDTO> products = new List<ProductDTO>();
            List<ParameterDTO> parameters = new List<ParameterDTO>();

            foreach(var importedProduct in import.Products)
            {
                var aliasesString = importedProduct.GetValueOrDefault("category.aliases");
                var aliasNames = aliasesString.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(name => name.Trim())
                    .ToList();
                ProductDTO product = new ProductDTO();

                product.Id = Guid.NewGuid();
                product.Name = importedProduct.GetValueOrDefault("name");
                product.Description = importedProduct.GetValueOrDefault("description");
                string priceString = importedProduct.GetValueOrDefault("price");

                if (!string.IsNullOrWhiteSpace(priceString)
                    && decimal.TryParse(priceString, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedPrice))
                    product.Price = decimal.Parse(importedProduct.GetValueOrDefault("price"));
                else 
                    product.Price = 0;

                product.Sku = importedProduct.GetValueOrDefault("sku");
                product.Ean = importedProduct.GetValueOrDefault("ean");
                product.Brand = new() { Name = importedProduct.GetValueOrDefault("brand") };

                product.Category = new() { Aliases = aliasNames.Select(name => new CategoryAliasDTO() { Name = name}).ToList(),
                    BaselinkerId = Int32.Parse(importedProduct.GetValueOrDefault("category.baselinkerId")), 
                    BaselinkerName = importedProduct.GetValueOrDefault("category.baselinkerName") };

                product.Parameters = new List<ParameterDTO>();

                foreach (var prop in import.ParameterMappings)
                {
                    product.Parameters.Add(new ParameterDTO()
                    {
                        Id = Guid.NewGuid(),
                        Name = prop.Key,
                        Value = importedProduct.GetValueOrDefault(prop.Key)
                    });
                }

                products.Add(product);
            }

            return products;
        }
    }
}
