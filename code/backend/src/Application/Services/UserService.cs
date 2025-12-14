using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using CVSystem.Application.Contracts.Services;
using CVSystem.Application.DTOs;
using CVSystem.Domain.Entities;

namespace CVSystem.Application.Services
{
    /// <summary>
    /// خدمة إدارة المستخدمين
    /// </summary>
    public class UserService : DomainService, IUserService
    {
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IRepository<UserProfile, Guid> _userProfileRepository;

        public UserService(
            IRepository<User, Guid> userRepository,
            IRepository<UserProfile, Guid> userProfileRepository)
        {
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
        }

        /// <summary>
        /// الحصول على مستخدم بواسطة المعرف
        /// </summary>
        public async Task<UserDto> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return null;
            }

            return MapToUserDto(user);
        }

        /// <summary>
        /// الحصول على مستخدم بواسطة البريد الإلكتروني
        /// </summary>
        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return null;
            }

            return MapToUserDto(user);
        }

        /// <summary>
        /// الحصول على مستخدم بواسطة اسم المستخدم
        /// </summary>
        public async Task<UserDto> GetUserByUserNameAsync(string userName)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                return null;
            }

            return MapToUserDto(user);
        }

        /// <summary>
        /// الحصول على جميع المستخدمين
        /// </summary>
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetListAsync();
            return users.Select(MapToUserDto).ToList();
        }

        /// <summary>
        /// البحث عن المستخدمين
        /// </summary>
        public async Task<List<UserDto>> SearchUsersAsync(string searchTerm)
        {
            var users = await _userRepository.GetListAsync(u =>
                u.FullName.Contains(searchTerm) ||
                u.Email.Contains(searchTerm) ||
                u.UserName.Contains(searchTerm));

            return users.Select(MapToUserDto).ToList();
        }

        /// <summary>
        /// تحديث بيانات المستخدم
        /// </summary>
        public async Task<UserDto> UpdateUserAsync(Guid userId, UpdateUserDto updateDto)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new ArgumentException("المستخدم غير موجود");
            }

            // تحديث البيانات الأساسية
            if (!string.IsNullOrWhiteSpace(updateDto.FullName))
            {
                user.UpdateFullName(updateDto.FullName);
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Email))
            {
                // التحقق من عدم وجود مستخدم آخر بنفس البريد الإلكتروني
                var existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Email == updateDto.Email && u.Id != userId);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("البريد الإلكتروني مستخدم مسبقاً");
                }
                user.UpdateEmail(updateDto.Email);
            }

            if (!string.IsNullOrWhiteSpace(updateDto.UserName))
            {
                // التحقق من عدم وجود مستخدم آخر بنفس اسم المستخدم
                var existingUser = await _userRepository.FirstOrDefaultAsync(u => u.UserName == updateDto.UserName && u.Id != userId);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("اسم المستخدم مستخدم مسبقاً");
                }
                user.UpdateUserName(updateDto.UserName);
            }

            if (updateDto.DateOfBirth.HasValue)
            {
                user.DateOfBirth = updateDto.DateOfBirth.Value;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Nationality))
            {
                user.Nationality = updateDto.Nationality;
            }

            await _userRepository.UpdateAsync(user, autoSave: true);

            return MapToUserDto(user);
        }

        /// <summary>
        /// تحديث الملف الشخصي للمستخدم
        /// </summary>
        public async Task<UserProfileDto> UpdateUserProfileAsync(Guid userId, UpdateUserProfileDto updateDto)
        {
            var userProfile = await _userProfileRepository.FirstOrDefaultAsync(p => p.UserId == userId);
            if (userProfile == null)
            {
                // إنشاء ملف شخصي جديد إذا لم يكن موجوداً
                userProfile = new UserProfile(userId);
                await _userProfileRepository.InsertAsync(userProfile, autoSave: true);
            }

            // تحديث بيانات الملف الشخصي
            if (!string.IsNullOrWhiteSpace(updateDto.PhoneNumber))
            {
                userProfile.PhoneNumber = updateDto.PhoneNumber;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Address))
            {
                userProfile.Address = updateDto.Address;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.ProfileImageUrl))
            {
                userProfile.ProfileImageUrl = updateDto.ProfileImageUrl;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Bio))
            {
                userProfile.Bio = updateDto.Bio;
            }

            await _userProfileRepository.UpdateAsync(userProfile, autoSave: true);

            return MapToUserProfileDto(userProfile);
        }

        /// <summary>
        /// الحصول على الملف الشخصي للمستخدم
        /// </summary>
        public async Task<UserProfileDto> GetUserProfileAsync(Guid userId)
        {
            var userProfile = await _userProfileRepository.FirstOrDefaultAsync(p => p.UserId == userId);
            if (userProfile == null)
            {
                return null;
            }

            return MapToUserProfileDto(userProfile);
        }

        /// <summary>
        /// حذف المستخدم
        /// </summary>
        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            // حذف الملف الشخصي أولاً
            var userProfile = await _userProfileRepository.FirstOrDefaultAsync(p => p.UserId == userId);
            if (userProfile != null)
            {
                await _userProfileRepository.DeleteAsync(userProfile);
            }

            // حذف المستخدم
            await _userRepository.DeleteAsync(user);

            return true;
        }

        /// <summary>
        /// تغيير حالة المستخدم (نشط/غير نشط)
        /// </summary>
        public async Task<bool> ToggleUserStatusAsync(Guid userId)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            user.ToggleStatus();
            await _userRepository.UpdateAsync(user, autoSave: true);

            return user.IsActive;
        }

        /// <summary>
        /// الحصول على عدد المستخدمين
        /// </summary>
        public async Task<int> GetUserCountAsync()
        {
            return await _userRepository.GetCountAsync();
        }

        /// <summary>
        /// الحصول على المستخدمين النشطين
        /// </summary>
        public async Task<List<UserDto>> GetActiveUsersAsync()
        {
            var users = await _userRepository.GetListAsync(u => u.IsActive);
            return users.Select(MapToUserDto).ToList();
        }

        /// <summary>
        /// الحصول على المستخدمين غير النشطين
        /// </summary>
        public async Task<List<UserDto>> GetInactiveUsersAsync()
        {
            var users = await _userRepository.GetListAsync(u => !u.IsActive);
            return users.Select(MapToUserDto).ToList();
        }

        /// <summary>
        /// الحصول على المستخدمين الذين سجلوا دخولهم مؤخراً
        /// </summary>
        public async Task<List<UserDto>> GetRecentlyLoggedInUsersAsync(int days = 7)
        {
            var dateThreshold = DateTime.UtcNow.AddDays(-days);
            var users = await _userRepository.GetListAsync(u => u.LastLoginDate.HasValue && u.LastLoginDate.Value >= dateThreshold);
            
            return users
                .OrderByDescending(u => u.LastLoginDate)
                .Select(MapToUserDto)
                .ToList();
        }

        /// <summary>
        /// التحقق من صلاحيات المستخدم
        /// </summary>
        public async Task<bool> HasPermissionAsync(Guid userId, string permission)
        {
            // TODO: تنفيذ منطق التحقق من الصلاحيات
            // يمكن دمج هذا مع نظام الصلاحيات في ABP Framework
            return true;
        }

        /// <summary>
        /// إضافة صلاحية للمستخدم
        /// </summary>
        public async Task<bool> AddPermissionAsync(Guid userId, string permission)
        {
            // TODO: تنفيذ منطق إضافة الصلاحيات
            return true;
        }

        /// <summary>
        /// إزالة صلاحية من المستخدم
        /// </summary>
        public async Task<bool> RemovePermissionAsync(Guid userId, string permission)
        {
            // TODO: تنفيذ منطق إزالة الصلاحيات
            return true;
        }

        /// <summary>
        /// الحصول على صلاحيات المستخدم
        /// </summary>
        public async Task<List<string>> GetUserPermissionsAsync(Guid userId)
        {
            // TODO: تنفيذ منطق الحصول على الصلاحيات
            return new List<string>();
        }

        /// <summary>
        /// تحويل User إلى UserDto
        /// </summary>
        private UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                IsActive = user.IsActive,
                DateOfBirth = user.DateOfBirth,
                Nationality = user.Nationality,
                FailedLoginAttempts = user.FailedLoginAttempts,
                LastLoginDate = user.LastLoginDate,
                LockedUntil = user.LockedUntil,
                CreationTime = user.CreationTime,
                UpdatedAt = user.UpdatedAt
            };
        }

        /// <summary>
        /// تحويل UserProfile إلى UserProfileDto
        /// </summary>
        private UserProfileDto MapToUserProfileDto(UserProfile userProfile)
        {
            return new UserProfileDto
            {
                Id = userProfile.Id,
                UserId = userProfile.UserId,
                PhoneNumber = userProfile.PhoneNumber,
                Address = userProfile.Address,
                ProfileImageUrl = userProfile.ProfileImageUrl,
                Bio = userProfile.Bio,
                CreatedAt = userProfile.CreatedAt,
                UpdatedAt = userProfile.UpdatedAt
            };
        }
    }

    /// <summary>
    /// بيانات تحديث المستخدم
    /// </summary>
    public class UpdateUserDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Nationality { get; set; }
    }

    /// <summary>
    /// بيانات تحديث الملف الشخصي
    /// </summary>
    public class UpdateUserProfileDto
    {
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Bio { get; set; }
    }
}