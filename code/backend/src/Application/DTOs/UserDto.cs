using System;
using Volo.Abp.Application.Dtos;

namespace CVSystem.Application.DTOs
{
    /// <summary>
    /// DTO للمستخدم
    /// </summary>
    public class UserDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// الاسم الكامل
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// اسم المستخدم
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// البريد الإلكتروني
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// هل البريد الإلكتروني مؤكد؟
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// رقم الهاتف
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// هل رقم الهاتف مؤكد؟
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// تاريخ الميلاد
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// الجنسية
        /// </summary>
        public string Nationality { get; set; }

        /// <summary>
        /// هل الحساب نشط؟
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// عدد محاولات الدخول الفاشلة
        /// </summary>
        public int FailedLoginAttempts { get; set; }

        /// <summary>
        /// تاريخ آخر دخول
        /// </summary>
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// تاريخ قفل الحساب
        /// </summary>
        public DateTime? LockoutEndDate { get; set; }

        /// <summary>
        /// عدد السير الذاتية
        /// </summary>
        public int CVCount { get; set; }

        /// <summary>
        /// تاريخ إنشاء الحساب
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// التحقق مما إذا كان الحساب مقفلاً
        /// </summary>
        public bool IsLocked
        {
            get
            {
                return LockoutEndDate.HasValue && LockoutEndDate > DateTime.UtcNow;
            }
        }

        /// <summary>
        /// الحصول على العمر (إذا كان تاريخ الميلاد متوفراً)
        /// </summary>
        public int? Age
        {
            get
            {
                if (!DateOfBirth.HasValue)
                    return null;

                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Value.Year;
                
                if (DateOfBirth.Value.Date > today.AddYears(-age))
                    age--;

                return age;
            }
        }

        /// <summary>
        /// الحصول على حالة الحساب
        /// </summary>
        public string AccountStatus
        {
            get
            {
                if (!IsActive)
                    return "معطل";

                if (IsLocked)
                    return "مقفل";

                return "نشط";
            }
        }

        /// <summary>
        /// الحصول على وقت آخر نشاط
        /// </summary>
        public string LastActivity
        {
            get
            {
                if (LastLoginDate.HasValue)
                {
                    var timeSince = DateTime.UtcNow - LastLoginDate.Value;
                    
                    if (timeSince.TotalDays > 30)
                        return $"آخر دخول منذ {Math.Floor(timeSince.TotalDays)} يوم";
                    else if (timeSince.TotalDays > 1)
                        return $"آخر دخول منذ {Math.Floor(timeSince.TotalDays)} أيام";
                    else if (timeSince.TotalHours > 1)
                        return $"آخر دخول منذ {Math.Floor(timeSince.TotalHours)} ساعة";
                    else
                        return $"آخر دخول منذ {Math.Floor(timeSince.TotalMinutes)} دقيقة";
                }

                return "لم يسجل دخول من قبل";
            }
        }

        /// <summary>
        /// التحقق من صلاحية بيانات المستخدم
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(FullName) &&
                   !string.IsNullOrWhiteSpace(UserName) &&
                   !string.IsNullOrWhiteSpace(Email);
        }

        /// <summary>
        /// الحصول على معلومات مختصرة عن المستخدم
        /// </summary>
        public string GetSummary()
        {
            return $"{FullName} ({Email}) - {AccountStatus}";
        }
    }
}