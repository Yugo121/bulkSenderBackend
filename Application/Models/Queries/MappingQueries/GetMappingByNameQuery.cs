using Application.Models.DTO_s;
using MediatR;

namespace Application.Models.Queries.MappingQueries
{
    public record GetMappingByNameQuery(string name) : IRequest<MappingDTO>;
}
