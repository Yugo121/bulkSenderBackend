using MediatR;

namespace Application.Models.Commands.SecretsCommands
{
    public record UpsertSecretCommand(string secretName, string secretValue) : IRequest<Unit>;
}
