using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands.CategoryCommands
{
    public record AddCategoryCommand(CategoryDTO category) : IRequest<Guid>;
}
