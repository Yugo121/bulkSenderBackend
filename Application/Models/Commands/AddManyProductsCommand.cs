using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands
{
    public record AddManyProductsCommand(List<ProductDTO> Products) : IRequest<List<Guid>>;
}
