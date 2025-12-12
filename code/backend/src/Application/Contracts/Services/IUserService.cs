using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CVSystem.Application.DTOs;

namespace CVSystem.Application.Contracts.Services
{
    /// <summary>
    /// عقد خدمة المستخدمين
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// الحصول على مستخدم بواسطة المعرف
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <returns>معلومات المستخدم</returns>
        Task<UserDto> GetUserByIdAsync(Guid userId);

        /// <summary>
        /// الحصول على مستخدم بواسطة البريد الإلكتروني
        /// </summary>
        /// <param name="email">البريد الإلكتروني</param>
        /// <returns>معلومات المستخدم</returns>
        Task<UserDto> GetUserByEmailAsync(string email);

        /// <summary>
        /// الحصول على مستخدم بواسطة اسم المستخدم
        /// </summary>
        /// <param name="userName">اسم المستخدم</param>
        /// <returns>معلومات المستخدم</returns>
        Task<UserDto> GetUserByUserNameAsync(string userName);

        /// <summary>
        /// تحديث معلومات المستخدم
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <param name="updateUserDto">بيانات التحديث</param>
        /// <returns>معلومات المستخدم المحدثة</returns>
        Task<UserDto> UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto);

        /// <summary>
        /// تحديث الملف الشخصي للمستخدم
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <param name="updateProfileDto">بيانات تحديث الملف الشخصي</param>
        /// <returns>الملف الشخصي المحدث</returns>
        Task<UserProfileDto> UpdateProfileAsync(Guid userId, UpdateProfileDto updateProfileDto);

        /// <summary>
        /// الحصول على الملف الشخصي للمستخدم
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <returns>الملف الشخصي</returns>
        Task<UserProfileDto> GetProfileAsync(Guid userId);

        /// <summary>
        /// حذف حساب المستخدم
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <returns>هل تم الحذف بنجاح؟</returns>
        Task<bool> DeleteUserAsync(Guid userId);

        /// <summary>
        /// تعطيل حساب المستخدم
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <returns>هل تم التعطيل بنجاح؟</returns>
        Task<bool> DeactivateUserAsync(Guid userId);

        /// <summary>
        /// تفعيل حساب المستخدم
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <returns>هل تم التفعيل بنجاح؟</returns>
        Task<bool> ActivateUserAsync(Guid userId);

        /// <summary>
        /// البحث عن المستخدمين
        /// </summary>
        /// <param name="searchTerm">مصطلح البحث</param>
        /// <param name="page">رقم الصفحة</param>
        /// <param name="pageSize">حجم الصفحة</param>
        /// <returns>قائمة المستخدمين</returns>
        Task<PagedResultDto<UserDto>> SearchUsersAsync(string searchTerm, int page = 1, int pageSize = 10);

        /// <summary>
        /// الحصول على جميع المستخدمين
        /// </summary>
        /// <param name="page">رقم الصفحة</param>
        /// <param name="pageSize">حجم الصفحة</param>
        /// <returns>قائمة المستخدمين</returns>
        Task<PagedResultDto<UserDto>> GetAllUsersAsync(int page = 1, int pageSize = 10);

        /// <summary>
        /// الحصول على عدد المستخدمين النشطين
        /// </summary>
        /// <returns>عدد المستخدمين النشطين</returns>
        Task<int> GetActiveUsersCountAsync();

        /// <summary>
        /// الحصول على إحصائيات المستخدمين
        /// </summary>
        /// <returns>إحصائيات المستخدمين</returns>
        Task<UserStatisticsDto> GetUserStatisticsAsync();

        /// <summary>
        /// تحديث صورة الملف الشخصي
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <param name="imageUrl">رابط الصورة</param>
        /// <returns>رابط الصورة المحدث</returns>
        Task<string> UpdateProfileImageAsync(Guid userId, string imageUrl);

        /// <summary>
        /// حذف صورة الملف الشخصي
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <returns>هل تم الحذف بنجاح؟</returns>
        Task<bool> DeleteProfileImageAsync(Guid userId);

        /// <summary>
        /// التحقق من صلاحية بيانات المستخدم
        /// </summary>
        /// <param name="userDto">بيانات المستخدم</param>
        /// <returns>رسائل التحقق من الصحة</returns>
        Task<List<string>> ValidateUserAsync(UserDto userDto);

        /// <summary>
        /// التحقق من صلاحية بيانات الملف الشخصي
        /// </summary>
        /// <param name="profileDto">بيانات الملف الشخصي</param>
        /// <returns>رسائل التحقق من الصحة</returns>
        Task<List<string>> ValidateProfileAsync(UserProfileDto profileDto);

        /// <summary>
        /// الحصول على معلومات المستخدم مع الملف الشخصي
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <returns>معلومات المستخدم الكاملة</returns>
        Task<UserWithProfileDto> GetUserWithProfileAsync(Guid userId);

        /// <summary>
        /// تحديث تفضيلات المستخدم
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <param name="preferencesDto">تفضيلات المستخدم</param>
        /// <returns>التفضيلات المحدثة</returns>
        Task<UserPreferencesDto> UpdatePreferencesAsync(Guid userId, UserPreferencesDto preferencesDto);

        /// <summary>
        /// الحصول على تفضيلات المستخدم
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <returns>تفضيلات المستخدم</returns>
        Task<UserPreferencesDto> GetPreferencesAsync(Guid userId);
    }
}