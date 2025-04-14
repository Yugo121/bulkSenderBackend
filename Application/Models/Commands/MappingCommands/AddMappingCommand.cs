using Application.Models.DTO_s;
using MediatR;

namespace Application.Models.Commands.MappingCommands
{
    public record AddMappingCommand(MappingDTO mapping) : IRequest<Guid>;
}
