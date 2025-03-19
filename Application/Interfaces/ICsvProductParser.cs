using Application.Models.Commands.ProductsCommands;

namespace Application.Interfaces
{
    public interface ICsvProductParser
    {
        List<AddProductCommand> ParseCsv(Stream csvStream, Dictionary<string, string> columnMappings);
    }
}
