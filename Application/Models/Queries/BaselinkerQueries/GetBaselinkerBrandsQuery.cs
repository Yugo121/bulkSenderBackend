using MediatR;

namespace Application.Models.Queries.BaselinkerQueries
{
    public record GetBaselinkerBrandsQuery : IRequest<string>;
}
