using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands
{
    public record AddBrandCommand(BrandDTO Brand) : IRequest<Guid>;
}
