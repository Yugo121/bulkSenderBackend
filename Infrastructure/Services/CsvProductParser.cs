using Application.Interfaces;
using Application.Models.Commands.ProductsCommands;
using Application.Models.DTOs;
using CsvHelper;
using System.Globalization;

namespace Infrastructure.Services
{
    public class CsvProductParser : ICsvProductParser
    {
        public AddManyProductsCommand ParseCsv(Stream csvStream, Dictionary<string, string> columnMappings)
        {
            using var reader = new StreamReader(csvStream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap(new DynamicCsvMap<ProductDTO>(columnMappings));

            List<ProductDTO> products = csv.GetRecords<ProductDTO>().ToList();

            AddManyProductsCommand addManyProductsCommand = new(products);

            return addManyProductsCommand;
        }
    }
}
