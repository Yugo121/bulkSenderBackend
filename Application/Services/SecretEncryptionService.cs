using Application.Interfaces;
using Microsoft.AspNetCore.DataProtection;

namespace Application.Services
{
    public class SecretEncryptionService : ISecretEncryptionService
    {
        private readonly IDataProtector _protector;

        public SecretEncryptionService(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("SecretsProtector");
        }

        public string Encrypt(string plaintext) => _protector.Protect(plaintext);
        public string Decrypt(string ciphertext) => _protector.Unprotect(ciphertext);
    }
}
