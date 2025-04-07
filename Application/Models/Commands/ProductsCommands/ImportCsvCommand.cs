using Application.Models.DTO_s;
using MediatR;

namespace Application.Models.Commands.ProductsCommands
{
    public record ImportCsvCommand(CsvImportRequest import) : IRequest;
}
