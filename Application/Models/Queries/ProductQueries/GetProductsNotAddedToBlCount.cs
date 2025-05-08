using MediatR;

namespace Application.Models.Queries.ProductQueries
{
    public record GetProductsNotAddedToBlCount : IRequest<int>;
}
