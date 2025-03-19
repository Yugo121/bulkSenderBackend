using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Queries.CategoryQueries
{
    public record GetCategoryQuery(Guid Id) : IRequest<CategoryDTO>;
}
