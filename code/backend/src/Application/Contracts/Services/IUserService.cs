using PsecKit.Core.Application.DTOs;
using System;
using System.Threading.Tasks;

namespace PsecKit.Core.Application.Contracts.Services
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(Guid userId);
        Task<UserProfileDto> GetUserProfileAsync(Guid userId);
        Task<UserDto> UpdateUserAsync(Guid userId, UserDto userDto);
        Task<UserProfileDto> UpdateUserProfileAsync(Guid userId, UserProfileDto userProfileDto);
        Task DeleteUserAsync(Guid userId);
    }
}
