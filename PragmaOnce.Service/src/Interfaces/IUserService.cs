using PragmaOnce.Core.src.Common;
using PragmaOnce.Service.src.DTOs;

namespace PragmaOnce.Service.src.Interfaces
{
    public interface IUserService : IBaseService<UserReadDto, UserCreateDto, UserUpdateDto, QueryOptions>
    {
        Task<bool> UpdatePasswordAsync(Guid userId, string newPassword);
        Task<UserReadDto> UpdateRoleAsync(Guid userId, UserRoleUpdateDto roleUpdateDto);
        Task<bool> ResetPasswordAsync(Guid userId, string newPassword);

    }
}