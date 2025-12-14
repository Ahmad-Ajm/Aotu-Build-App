using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace CVSystem.Domain.Entities
{
    /// <summary>
    /// كيان السيرة الذاتية الرئيسي
    /// </summary>
    public class CV : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// معرف المستخدم المالك للسيرة الذاتية
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// عنوان السيرة الذاتية
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// المعلومات الشخصية بتنسيق JSON
        /// </summary>
        public string PersonalInfo { get; set; }

        /// <summary>
        /// الخبرات العملية بتنسيق JSON
        /// </summary>
        public string WorkExperience { get; set; }

        /// <summary>
        /// التعليم والمؤهلات بتنسيق JSON
        /// </summary>
        public string Education { get; set; }

        /// <summary>
        /// المهارات بتنسيق JSON
        /// </summary>
        public string Skills { get; set; }

        /// <summary>
        /// معلومات الاتصال بتنسيق JSON
        /// </summary>
        public string ContactInfo { get; set; }

        /// <summary>
        /// القالب المستخدم
        /// </summary>
        public string Template { get; set; } = "professional";

        /// <summary>
        /// حالة السيرة الذاتية (عامة/خاصة)
        /// </summary>
        public bool IsPublic { get; set; } = false;

        /// <summary>
        /// رابط المشاركة الفريد
        /// </summary>
        public string ShareLink { get; set; }

        /// <summary>
        /// تاريخ آخر تحديث
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// عدد المشاهدات
        /// </summary>
        public int ViewCount { get; set; } = 0;

        /// <summary>
        /// البناء الافتراضي
        /// </summary>
        public CV()
        {
            Id = Guid.NewGuid();
            ShareLink = GenerateShareLink();
        }

        /// <summary>
        /// بناء مع المعلمات الأساسية
        /// </summary>
        public CV(Guid userId, string title) : this()
        {
            UserId = userId;
            Title = title ?? "سيرتي الذاتية";
        }

        /// <summary>
        /// توليد رابط مشاركة فريد
        /// </summary>
        private string GenerateShareLink()
        {
            return $"cv-{Id.ToString("N").Substring(0, 8)}";
        }

        /// <summary>
        /// تحديث تاريخ آخر تحديث
        /// </summary>
        public void UpdateLastUpdated()
        {
            LastUpdated = DateTime.UtcNow;
        }

        /// <summary>
        /// زيادة عدد المشاهدات
        /// </summary>
        public void IncrementViewCount()
        {
            ViewCount++;
        }

        /// <summary>
        /// تغيير حالة السيرة الذاتية
        /// </summary>
        public void ToggleVisibility()
        {
            IsPublic = !IsPublic;
            UpdateLastUpdated();
        }

        /// <summary>
        /// التحقق من صحة البيانات الأساسية
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Title) && UserId != Guid.Empty;
        }
    }
}