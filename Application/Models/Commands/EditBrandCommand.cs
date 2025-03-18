using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands
{
    public record EditBrandCommand(BrandDTO Brand) : IRequest<Guid>;
}
