using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Queries.SecretQueries
{
    public class GetSecretHandler : IRequestHandler<GetSecretQuery, string>
    {
        private readonly ISecretEncryptionService _secretEncryptionService;
        private readonly IAppDbContext _context;
        public GetSecretHandler(ISecretEncryptionService secretEncryptionService, IAppDbContext context)
        {
            _secretEncryptionService = secretEncryptionService;
            _context = context;
        }
        public async Task<string> Handle(GetSecretQuery request, CancellationToken cancellationToken)
        {
            var secret = await _context.Secrets.FirstOrDefaultAsync(s => s.Name == request.secretName);

            if (secret == null)
            {
                throw new KeyNotFoundException($"Secret with name '{request.secretName}' not found.");
            }
            return _secretEncryptionService.Decrypt(secret.Value);
        }
    }
}
