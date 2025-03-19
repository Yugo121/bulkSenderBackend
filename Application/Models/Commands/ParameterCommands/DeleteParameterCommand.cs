using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands.ParameterCommands
{
    public record DeleteParameterCommand(Guid ParameterId) : IRequest<Guid>;
}
