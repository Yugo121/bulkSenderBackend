using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands.ParameterCommands
{
    public record AddParameterCommand(ParameterDTO Parameter) : IRequest<Guid>;
}
