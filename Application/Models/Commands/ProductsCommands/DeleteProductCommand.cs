using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands.ProductsCommands
{
    public record DeleteProductCommand(Guid ProductId) : IRequest<Guid>;
}
