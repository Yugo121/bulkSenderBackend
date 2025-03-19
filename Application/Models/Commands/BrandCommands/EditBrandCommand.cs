using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands.BrandCommands
{
    public record EditBrandCommand(BrandDTO Brand) : IRequest<Guid>;
}
