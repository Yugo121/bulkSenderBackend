
using MediatR;

namespace Application.Models.Queries.ParameterQueries
{
    public record GetParametersNamesQuery : IRequest<List<string>>;
}
