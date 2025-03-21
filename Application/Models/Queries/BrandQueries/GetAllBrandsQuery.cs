using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Queries.BrandQueries
{
    public record GetAllBrandsQuery : IRequest<List<BrandDTO>>;
}
