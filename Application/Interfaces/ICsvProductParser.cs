using Application.Models.Commands.ProductsCommands;

namespace Application.Interfaces
{
    public interface ICsvProductParser
    {
        AddManyProductsCommand ParseCsv(Stream csvStream, Dictionary<string, string> columnMappings);
    }
}
