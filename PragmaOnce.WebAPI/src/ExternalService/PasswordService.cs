using PragmaOnce.Core.src.Entities;
using PragmaOnce.Service.src.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace PragmaOnce.WebAPI.src.ExternalService
{
    public class PasswordService : IPasswordService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        public PasswordService(IPasswordHasher<User> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public string HashPassword(User user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public PasswordVerificationResult VerifyPassword(User user, string hashedPassword, string providedPassword)
        {
            return _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        }
    }
}