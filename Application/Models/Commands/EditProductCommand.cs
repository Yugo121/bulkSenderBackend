using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands
{
    public record EditProductCommand(ProductDTO Product) : IRequest<Guid>;
}
