using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Queries.ProductQueries
{
    public record GetProductsNotAddedToBaselinkerQuery(int page, int quantityPerPage) : IRequest<List<ProductDTO>>;
}
