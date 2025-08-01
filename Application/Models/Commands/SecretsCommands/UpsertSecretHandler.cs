using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Commands.SecretsCommands
{
    public class UpsertSecretHandler : IRequestHandler<UpsertSecretCommand, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly ISecretEncryptionService _secretEncryptionService;
        public UpsertSecretHandler(IAppDbContext context, ISecretEncryptionService encryptionService)
        {
            _context = context;
            _secretEncryptionService = encryptionService;
        }
        public async Task<Unit> Handle(UpsertSecretCommand request, CancellationToken cancellationToken)
        {
            var secret = await _context.Secrets.FirstOrDefaultAsync(s => s.Name == request.secretName);
            string encrypted = _secretEncryptionService.Encrypt(request.secretValue);

            if (secret == null)
            {
                secret = new SecretEntity {Id = Guid.NewGuid(), Name = request.secretName, Value = encrypted };
                _context.Secrets.Add(secret);
            }
            else
            {
                secret.Value = encrypted;
                _context.Secrets.Update(secret);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
