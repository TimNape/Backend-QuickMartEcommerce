using PragmaOnce.Core.src.Common;
using PragmaOnce.Service.src.DTOs;

namespace PragmaOnce.Service.src.Interfaces
{
    public interface IAuthService
    {
        Task<string> LogInAsync(UserCredential userCredential);
        Task<string> LogoutAsync();
        Task<UserReadDto> AuthenticateUserAsync(string token);
    }
}