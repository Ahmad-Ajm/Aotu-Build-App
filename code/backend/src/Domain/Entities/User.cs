using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace CVSystem.Domain.Entities
{
    /// <summary>
    /// كيان المستخدم (توسيع IdentityUser)
    /// </summary>
    public class User : IdentityUser<Guid>
    {
        /// <summary>
        /// الاسم الكامل للمستخدم
        /// </summary>
        public string FullName { get; set; }

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
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// عدد محاولات الدخول الفاشلة
        /// </summary>
        public int FailedLoginAttempts { get; set; }

        /// <summary>
        /// تاريخ آخر محاولة دخول
        /// </summary>
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// تاريخ قفل الحساب (إن وجد)
        /// </summary>
        public DateTime? LockoutEndDate { get; set; }

        /// <summary>
        /// السير الذاتية الخاصة بالمستخدم
        /// </summary>
        public virtual ICollection<CV> CVs { get; set; }

        /// <summary>
        /// الملف الشخصي للمستخدم
        /// </summary>
        public virtual UserProfile Profile { get; set; }

        /// <summary>
        /// مُنشئ افتراضي
        /// </summary>
        public User()
        {
            CVs = new HashSet<CV>();
        }

        /// <summary>
        /// مُنشئ مع المعلمات الأساسية
        /// </summary>
        public User(Guid id, string userName, string email, string fullName) 
            : base(id, userName, email)
        {
            FullName = fullName;
            IsActive = true;
            CVs = new HashSet<CV>();
        }

        /// <summary>
        /// زيادة عدد محاولات الدخول الفاشلة
        /// </summary>
        public void IncrementFailedLoginAttempts()
        {
            FailedLoginAttempts++;
            LastLoginDate = DateTime.UtcNow;
        }

        /// <summary>
        /// إعادة تعيين محاولات الدخول الفاشلة
        /// </summary>
        public void ResetFailedLoginAttempts()
        {
            FailedLoginAttempts = 0;
            LastLoginDate = DateTime.UtcNow;
        }

        /// <summary>
        /// قفل الحساب
        /// </summary>
        /// <param name="minutes">عدد الدقائق للقفل</param>
        public void LockAccount(int minutes = 30)
        {
            LockoutEndDate = DateTime.UtcNow.AddMinutes(minutes);
        }

        /// <summary>
        /// فتح الحساب
        /// </summary>
        public void UnlockAccount()
        {
            LockoutEndDate = null;
            FailedLoginAttempts = 0;
        }

        /// <summary>
        /// التحقق مما إذا كان الحساب مقفلاً
        /// </summary>
        public bool IsLocked()
        {
            return LockoutEndDate.HasValue && LockoutEndDate > DateTime.UtcNow;
        }

        /// <summary>
        /// تحديث تاريخ آخر دخول
        /// </summary>
        public void UpdateLastLogin()
        {
            LastLoginDate = DateTime.UtcNow;
            ResetFailedLoginAttempts();
        }

        /// <summary>
        /// التحقق من صلاحية بيانات المستخدم
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(UserName) && 
                   !string.IsNullOrWhiteSpace(Email) && 
                   !string.IsNullOrWhiteSpace(FullName);
        }
    }
}