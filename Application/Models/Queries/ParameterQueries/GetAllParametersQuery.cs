using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Queries.ParameterQueries
{
    public record GetAllParametersQuery : IRequest<List<ParameterDTO>>;
}
