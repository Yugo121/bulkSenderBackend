using MediatR;

namespace Application.Models.Queries.SecretQueries
{
    public record GetSecretQuery(string secretName) : IRequest<string>;
}
