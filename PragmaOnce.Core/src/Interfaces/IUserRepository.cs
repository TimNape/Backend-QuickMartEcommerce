using PragmaOnce.Core.src.Common;
using PragmaOnce.Core.src.Entities;
using PragmaOnce.Core.src.ValueObjects;

namespace PragmaOnce.Core.src.Interfaces
{
    public interface IUserRepository : IBaseRepository<User, QueryOptions>
    {
        // Get a user by their email address
        Task<User> GetByEmailAsync(string email);

        // Update a user's role
        Task<bool> UpdateUserRoleAsync(Guid userId, UserRole newRole);

        // Reset a user's password
        Task<bool> ResetPasswordAsync(Guid userId, string newPassword);

        // Get a user by their credentials
        Task<User> GetUserByCredentialsAsync(UserCredential userCredential);

    }
}