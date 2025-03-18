using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands
{
    public record EditCategoryCommand(CategoryDTO Category) : IRequest<Guid>;
}
