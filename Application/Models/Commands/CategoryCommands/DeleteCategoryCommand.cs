using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands.CategoryCommands
{
    public record DeleteCategoryCommand(Guid CategoryId) : IRequest<Guid>;
}
