using MediatR;

namespace Application.Models.Commands.ProductsCommands
{
    public record ImportCsvCommand(Stream csvStream, Dictionary<string, string> columnMappings) : IRequest;
}
