using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands.CategoryCommands
{
    public record EditCategoryCommand(CategoryDTO Category) : IRequest<Guid>;
}
