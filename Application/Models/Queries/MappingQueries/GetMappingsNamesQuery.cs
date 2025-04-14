using MediatR;

namespace Application.Models.Queries.MappingQueries
{
    public record GetMappingsNamesQuery : IRequest<List<string>>;
}