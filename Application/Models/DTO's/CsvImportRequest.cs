using Microsoft.AspNetCore.Http;

namespace Application.Models.DTO_s
{
    public class CsvImportRequest
    {
        public Dictionary<string, string> ColumnMappings { get; set; }
        public IFormFile CsvFile { get; set; }
    }
}
