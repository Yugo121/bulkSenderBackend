using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands
{
    public record AddCategoryCommand(CategoryDTO category) : IRequest<Guid>;
}
