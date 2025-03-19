using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands.ProductsCommands
{
    public record AddProductCommand(ProductDTO Product) : IRequest<Guid>;
}
