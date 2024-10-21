using PragmaOnce.Core.src.Entities;
using Microsoft.AspNetCore.Identity;

namespace PragmaOnce.Service.src.Interfaces
{
    public interface IPasswordService
    {
        string HashPassword(User user, string password);
        PasswordVerificationResult VerifyPassword(User user, string hashedPassword, string providedPassword);
    }
}