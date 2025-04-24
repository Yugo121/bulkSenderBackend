using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands.BaselinkerCommands
{
    public record AddProductToBaselinkerCommand(ProductDTO product) : IRequest<int>;
}
