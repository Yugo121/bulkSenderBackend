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
                
                ProductDTO product = new ProductDTO();

                product.Id = Guid.NewGuid();
                product.Name = importedProduct.GetValueOrDefault("name");
                product.Description = importedProduct.GetValueOrDefault("description");
                product.Price = decimal.Parse(importedProduct.GetValueOrDefault("price"));
                product.Sku = importedProduct.GetValueOrDefault("sku");
                product.Ean = importedProduct.GetValueOrDefault("ean");
                product.Brand = new() { Name = importedProduct.GetValueOrDefault("brand") };
                product.Category = new() { Name = importedProduct.GetValueOrDefault("category.name"), BaselinkerId = Int32.Parse(importedProduct.GetValueOrDefault("category.baselinkerId")) };
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
