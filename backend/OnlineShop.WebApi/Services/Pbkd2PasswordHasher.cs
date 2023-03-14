using Microsoft.AspNetCore.Identity;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Services;

namespace OnlineShop.WebApi.Services
{
    public class Pbkd2PasswordHasher : IPasswordHasherService
    {
        private readonly PasswordHasher<Account> _hasher = new();

        private readonly Account _fakeAccount = new Account(
            Guid.Empty, "", "fake@fake.com", "");

       

        public string HashPassword(string password)
        {
            var hasher = new PasswordHasher<Account>();
            string hashedPassword = _hasher.HashPassword(_fakeAccount, password);
            return hashedPassword;
        }

        public bool VerifyPassword(string passwordHash, string password)
        {
            var result = _hasher.VerifyHashedPassword(
                _fakeAccount, passwordHash, password);
            return result is not PasswordVerificationResult.Failed;
        }
    }
}