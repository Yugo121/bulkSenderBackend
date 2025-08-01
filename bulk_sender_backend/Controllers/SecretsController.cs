using Application.Models.Commands.SecretsCommands;
using Application.Models.Queries.SecretQueries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace bulk_sender_backend.Controllers
{
    public class SecretsController : Controller
    {
        private readonly IMediator _mediator;

        public SecretsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("api/secrets")]
        public async Task<IActionResult> AddSecret([FromBody] List<SecretEntity> secrets)
        {
            if (secrets == null || !secrets.Any())
            {
                return BadRequest("No secrets provided.");
            }
            foreach (var secret in secrets)
            {
                if (string.IsNullOrEmpty(secret.Name) || string.IsNullOrEmpty(secret.Value))
                {
                    return BadRequest("Secret name and value cannot be empty.");
                }

                await _mediator.Send(new UpsertSecretCommand(secret.Name, secret.Value));

            }

            return Ok("Secrets added successfully.");
        }

        [HttpGet]
        [Route("api/secrets/{secretName}")]
        public async Task<IActionResult> GetSecret(string secretName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(secretName))
            {
                return BadRequest("Secret name cannot be empty.");
            }
            try
            {
                var secretValue = await _mediator.Send(new GetSecretQuery(secretName), cancellationToken);
                return Ok(secretValue);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Secret with name '{secretName}' not found.");
            }
        }
    }
}
