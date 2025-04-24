using MediatR;

namespace Application.Models.Queries.CategoryQueries
{
    public record GetCategoriesNamesQuery : IRequest<List<string>>;
}
