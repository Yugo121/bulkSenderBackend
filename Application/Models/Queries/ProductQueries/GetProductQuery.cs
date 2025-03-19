using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Queries.ProductQueries
{
    public record GetProductQuery(Guid Id) : IRequest<ProductDTO>;
}
