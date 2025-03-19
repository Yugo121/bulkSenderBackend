using Application.Interfaces;
using Application.Models.Commands.ProductsCommands;
using CsvHelper;
using System.Globalization;

namespace Infrastructure.Services
{
    public class CsvProductParser : ICsvProductParser
    {
        public List<AddProductCommand> ParseCsv(Stream csvStream, Dictionary<string, string> columnMappings)
        {
            using var reader = new StreamReader(csvStream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            // Dynamiczna mapa kolumn
            foreach (var mapping in columnMappings)
            {
                csv.Context.RegisterClassMap(new DynamicCsvMap<AddProductCommand>(columnMappings));
            }

            return csv.GetRecords<AddProductCommand>().ToList();
        }
    }
}
