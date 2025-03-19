using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands.ParameterCommands
{
    public record EditParameterCommand(ParameterDTO Parameter) : IRequest<Guid>;
}
