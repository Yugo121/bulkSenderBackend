using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands.BrandCommands
{
    public record DeleteBrandCommand(Guid BrandId) : IRequest<Guid>;
}
