using MediatR;
namespace Application.Models.Queries.BrandQueries
{
    public record GetBrandsNamesQuery : IRequest<List<string>>;
}
