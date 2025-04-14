using Application.Models.DTO_s;
using MediatR;

namespace Application.Models.Queries.MappingQueries
{
    public record GetMappingQuery(Guid id) : IRequest<MappingDTO>;
}
