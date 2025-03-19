using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands.ProductsCommands
{
    public record EditProductCommand(ProductDTO Product) : IRequest<Guid>;
}
