using PragmaOnce.Core.src.Entities;

namespace PragmaOnce.Service.src.Interfaces
{
    public interface ITokenService
    {
        public string GetToken(User user);
        public Guid VerifyToken(string token);
        Task<string> InvalidateTokenAsync();
    }
}