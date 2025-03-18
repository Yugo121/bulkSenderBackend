using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands
{
    public record AddParameterCommand(ParameterDTO Parameter) : IRequest<Guid>;
}
