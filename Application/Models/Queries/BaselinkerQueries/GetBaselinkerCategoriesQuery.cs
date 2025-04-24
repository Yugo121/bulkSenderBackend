using MediatR;

namespace Application.Models.Queries.BaselinkerQueries
{
    public record GetBaselinkerCategoriesQuery : IRequest<string>;
}
