using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands.ProductsCommands
{
    public record AddManyProductsCommand(List<ProductDTO> Products) : IRequest<List<Guid>>;
}
