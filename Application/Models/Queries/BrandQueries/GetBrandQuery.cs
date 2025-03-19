using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Queries.BrandQueries
{
    public record GetBrandQuery(Guid id) : IRequest<BrandDTO>;
}
