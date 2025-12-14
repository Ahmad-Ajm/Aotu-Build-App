using System;

namespace CVSystem.Application.DTOs
{
    /// <summary>
    /// DTO لاستجابة المصادقة
    /// </summary>
    public class AuthResponseDto
    {
        /// <summary>
        /// معرف المستخدم
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// الاسم الكامل
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// البريد الإلكتروني
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// اسم المستخدم
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// توكن الوصول (JWT)
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// نوع التوكن
        /// </summary>
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// تاريخ انتهاء صلاحية التوكن
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// توكن التحديث (للتجديد)
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// هل المستخدم نشط؟
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// تاريخ آخر دخول
        /// </summary>
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// مُنشئ افتراضي
        /// </summary>
        public AuthResponseDto()
        {
        }

        /// <summary>
        /// مُنشئ مع المعلمات الأساسية
        /// </summary>
        public AuthResponseDto(Guid userId, string fullName, string email, string userName, string accessToken, DateTime expiresAt)
        {
            UserId = userId;
            FullName = fullName;
            Email = email;
            UserName = userName;
            AccessToken = accessToken;
            ExpiresAt = expiresAt;
            IsActive = true;
            LastLoginDate = DateTime.UtcNow;
        }

        /// <summary>
        /// مُنشئ كامل
        /// </summary>
        public AuthResponseDto(Guid userId, string fullName, string email, string userName, 
                              string accessToken, string refreshToken, DateTime expiresAt, 
                              bool isActive, DateTime? lastLoginDate)
        {
            UserId = userId;
            FullName = fullName;
            Email = email;
            UserName = userName;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpiresAt = expiresAt;
            IsActive = isActive;
            LastLoginDate = lastLoginDate;
        }

        /// <summary>
        /// التحقق من صلاحية التوكن
        /// </summary>
        public bool IsTokenValid()
        {
            return !string.IsNullOrWhiteSpace(AccessToken) && ExpiresAt > DateTime.UtcNow;
        }

        /// <summary>
        /// الحصول على الوقت المتبقي للتوكن
        /// </summary>
        public TimeSpan GetRemainingTime()
        {
            return ExpiresAt - DateTime.UtcNow;
        }

        /// <summary>
        /// الحصول على رسالة حالة التوكن
        /// </summary>
        public string GetTokenStatus()
        {
            if (string.IsNullOrWhiteSpace(AccessToken))
                return "لا يوجد توكن";

            if (ExpiresAt <= DateTime.UtcNow)
                return "منتهي الصلاحية";

            var remaining = GetRemainingTime();
            if (remaining.TotalHours < 1)
                return $"ينتهي خلال {remaining.Minutes} دقيقة";
            else if (remaining.TotalDays < 1)
                return $"ينتهي خلال {remaining.Hours} ساعة";
            else
                return $"ينتهي خلال {remaining.Days} يوم";
        }
    }
}