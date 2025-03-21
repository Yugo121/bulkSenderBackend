using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Queries.CategoryQueries
{
    public record GetAllCategoriesQuery : IRequest<List<CategoryDTO>>;
}
