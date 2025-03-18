using Application.Models.DTOs;
using MediatR;

namespace Application.Models.Commands
{
    public record EditParameterCommand(ParameterDTO Parameter) : IRequest<Guid>;
}
