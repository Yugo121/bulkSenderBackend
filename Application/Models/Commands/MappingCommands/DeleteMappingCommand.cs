using MediatR;

namespace Application.Models.Commands.MappingCommands
{
    public record DeleteMappingCommand(Guid id) : IRequest<Guid>;
}
