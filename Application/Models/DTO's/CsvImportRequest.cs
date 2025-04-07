using Microsoft.AspNetCore.Http;

namespace Application.Models.DTO_s
{
    public class CsvImportRequest
    {
        public Dictionary<string, string> ProductMappings { get; set; } = new();
        public Dictionary<string, string> ParameterMappings { get; set; } = new();

        public List<Dictionary<string, string>> Products { get; set; } = new();

    }
}
