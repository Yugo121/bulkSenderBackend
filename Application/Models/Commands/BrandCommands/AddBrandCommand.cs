using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands.BrandCommands
{
    public record AddBrandCommand(BrandDTO Brand) : IRequest<Guid>;
}
