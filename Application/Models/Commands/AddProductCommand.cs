using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands
{
    public record AddProductCommand(ProductDTO Product) : IRequest<Guid>;
}
