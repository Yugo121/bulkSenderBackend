using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Queries.ProductQueries
{
    public record GetManyProductsQuery(int quantity, int page) : IRequest<List<ProductDTO>>;
}
