using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Queries.ParameterQueries
{
    public record GetParameterQuery(Guid Id) : IRequest<ParameterDTO>;
}
