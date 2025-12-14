using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace CVSystem.Domain.Entities
{
    /// <summary>
    /// الملف الشخصي للمستخدم
    /// </summary>
    public class UserProfile : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// معرف المستخدم
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// المستخدم
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// رقم الهاتف
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// العنوان
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// المدينة
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// الدولة
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// الرمز البريدي
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// صورة الملف الشخصي
        /// </summary>
        public string ProfileImageUrl { get; set; }

        /// <summary>
        /// نبذة عن المستخدم
        /// </summary>
        public string Bio { get; set; }

        /// <summary>
        /// الموقع الإلكتروني
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// رابط LinkedIn
        /// </summary>
        public string LinkedInUrl { get; set; }

        /// <summary>
        /// رابط GitHub
        /// </summary>
        public string GitHubUrl { get; set; }

        /// <summary>
        /// رابط Twitter
        /// </summary>
        public string TwitterUrl { get; set; }

        /// <summary>
        /// تفضيلات اللغة
        /// </summary>
        public string LanguagePreference { get; set; } = "ar";

        /// <summary>
        /// تفضيلات السمة (داكن/فاتح)
        /// </summary>
        public string ThemePreference { get; set; } = "light";

        /// <summary>
        /// هل يريد المستخدم تلقي إشعارات؟
        /// </summary>
        public bool ReceiveNotifications { get; set; } = true;

        /// <summary>
        /// هل يريد المستخدم تلقي رسائل بريدية؟
        /// </summary>
        public bool ReceiveEmails { get; set; } = true;

        /// <summary>
        /// تاريخ آخر تحديث للملف الشخصي
        /// </summary>
        public DateTime LastProfileUpdate { get; set; }

        /// <summary>
        /// مُنشئ افتراضي
        /// </summary>
        public UserProfile()
        {
            LastProfileUpdate = DateTime.UtcNow;
        }

        /// <summary>
        /// مُنشئ مع معرف المستخدم
        /// </summary>
        public UserProfile(Guid userId) : this()
        {
            UserId = userId;
        }

        /// <summary>
        /// تحديث تاريخ آخر تعديل للملف الشخصي
        /// </summary>
        public void UpdateLastProfileUpdate()
        {
            LastProfileUpdate = DateTime.UtcNow;
        }

        /// <summary>
        /// التحقق من صلاحية بيانات الملف الشخصي
        /// </summary>
        public bool IsValid()
        {
            return UserId != Guid.Empty;
        }

        /// <summary>
        /// الحصول على العنوان الكامل
        /// </summary>
        public string GetFullAddress()
        {
            var parts = new List<string>();
            
            if (!string.IsNullOrWhiteSpace(Address))
                parts.Add(Address);
            
            if (!string.IsNullOrWhiteSpace(City))
                parts.Add(City);
            
            if (!string.IsNullOrWhiteSpace(Country))
                parts.Add(Country);
            
            if (!string.IsNullOrWhiteSpace(PostalCode))
                parts.Add($"({PostalCode})");
            
            return string.Join(", ", parts);
        }

        /// <summary>
        /// الحصول على روابط التواصل الاجتماعي
        /// </summary>
        public Dictionary<string, string> GetSocialLinks()
        {
            var links = new Dictionary<string, string>();
            
            if (!string.IsNullOrWhiteSpace(LinkedInUrl))
                links.Add("LinkedIn", LinkedInUrl);
            
            if (!string.IsNullOrWhiteSpace(GitHubUrl))
                links.Add("GitHub", GitHubUrl);
            
            if (!string.IsNullOrWhiteSpace(TwitterUrl))
                links.Add("Twitter", TwitterUrl);
            
            if (!string.IsNullOrWhiteSpace(Website))
                links.Add("Website", Website);
            
            return links;
        }

        /// <summary>
        /// تحديث تفضيلات المستخدم
        /// </summary>
        public void UpdatePreferences(string language, string theme, bool notifications, bool emails)
        {
            LanguagePreference = language ?? "ar";
            ThemePreference = theme ?? "light";
            ReceiveNotifications = notifications;
            ReceiveEmails = emails;
            UpdateLastProfileUpdate();
        }
    }
}