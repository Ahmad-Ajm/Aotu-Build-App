using System;
using System.Threading.Tasks;
using CVSystem.Application.DTOs;

namespace CVSystem.Application.Contracts.Services
{
    /// <summary>
    /// عقد خدمة المصادقة
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// تسجيل مستخدم جديد
        /// </summary>
        /// <param name="registerDto">بيانات التسجيل</param>
        /// <returns>استجابة المصادقة</returns>
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);

        /// <summary>
        /// تسجيل الدخول
        /// </summary>
        /// <param name="loginDto">بيانات تسجيل الدخول</param>
        /// <returns>استجابة المصادقة</returns>
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);

        /// <summary>
        /// تسجيل الخروج
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <returns>هل تم تسجيل الخروج بنجاح؟</returns>
        Task<bool> LogoutAsync(Guid userId);

        /// <summary>
        /// تحديث توكن الوصول
        /// </summary>
        /// <param name="refreshToken">توكن التحديث</param>
        /// <returns>استجابة المصادقة الجديدة</returns>
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);

        /// <summary>
        /// التحقق من صحة التوكن
        /// </summary>
        /// <param name="token">توكن الوصول</param>
        /// <returns>هل التوكن صالح؟</returns>
        Task<bool> ValidateTokenAsync(string token);

        /// <summary>
        /// الحصول على معلومات المستخدم من التوكن
        /// </summary>
        /// <param name="token">توكن الوصول</param>
        /// <returns>معلومات المستخدم</returns>
        Task<UserDto> GetUserFromTokenAsync(string token);

        /// <summary>
        /// تغيير كلمة المرور
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <param name="currentPassword">كلمة المرور الحالية</param>
        /// <param name="newPassword">كلمة المرور الجديدة</param>
        /// <returns>هل تم تغيير كلمة المرور بنجاح؟</returns>
        Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);

        /// <summary>
        /// إعادة تعيين كلمة المرور (للمستخدمين الذين نسوا كلمة المرور)
        /// </summary>
        /// <param name="email">البريد الإلكتروني</param>
        /// <returns>هل تم إرسال رابط إعادة التعيين؟</returns>
        Task<bool> ForgotPasswordAsync(string email);

        /// <summary>
        /// التحقق من صحة رابط إعادة تعيين كلمة المرور
        /// </summary>
        /// <param name="token">توكن إعادة التعيين</param>
        /// <returns>هل الرابط صالح؟</returns>
        Task<bool> ValidateResetPasswordTokenAsync(string token);

        /// <summary>
        /// إعادة تعيين كلمة المرور باستخدام التوكن
        /// </summary>
        /// <param name="token">توكن إعادة التعيين</param>
        /// <param name="newPassword">كلمة المرور الجديدة</param>
        /// <returns>هل تم إعادة تعيين كلمة المرور بنجاح؟</returns>
        Task<bool> ResetPasswordAsync(string token, string newPassword);

        /// <summary>
        /// التحقق من وجود مستخدم بالبريد الإلكتروني
        /// </summary>
        /// <param name="email">البريد الإلكتروني</param>
        /// <returns>هل المستخدم موجود؟</returns>
        Task<bool> UserExistsByEmailAsync(string email);

        /// <summary>
        /// التحقق من وجود مستخدم باسم المستخدم
        /// </summary>
        /// <param name="userName">اسم المستخدم</param>
        /// <returns>هل المستخدم موجود؟</returns>
        Task<bool> UserExistsByUserNameAsync(string userName);

        /// <summary>
        /// الحصول على عدد محاولات الدخول الفاشلة
        /// </summary>
        /// <param name="emailOrUserName">البريد الإلكتروني أو اسم المستخدم</param>
        /// <returns>عدد المحاولات الفاشلة</returns>
        Task<int> GetFailedLoginAttemptsAsync(string emailOrUserName);

        /// <summary>
        /// التحقق مما إذا كان الحساب مقفلاً
        /// </summary>
        /// <param name="emailOrUserName">البريد الإلكتروني أو اسم المستخدم</param>
        /// <returns>هل الحساب مقفل؟</returns>
        Task<bool> IsAccountLockedAsync(string emailOrUserName);

        /// <summary>
        /// قفل الحساب مؤقتاً
        /// </summary>
        /// <param name="emailOrUserName">البريد الإلكتروني أو اسم المستخدم</param>
        /// <param name="minutes">عدد الدقائق للقفل</param>
        /// <returns>هل تم قفل الحساب؟</returns>
        Task<bool> LockAccountAsync(string emailOrUserName, int minutes = 30);

        /// <summary>
        /// فتح الحساب
        /// </summary>
        /// <param name="emailOrUserName">البريد الإلكتروني أو اسم المستخدم</param>
        /// <returns>هل تم فتح الحساب؟</returns>
        Task<bool> UnlockAccountAsync(string emailOrUserName);
    }
}