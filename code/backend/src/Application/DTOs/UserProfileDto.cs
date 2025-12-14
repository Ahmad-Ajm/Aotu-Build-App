using System;
using Volo.Abp.Application.Dtos;

namespace CVSystem.Application.DTOs
{
    /// <summary>
    /// DTO للملف الشخصي للمستخدم
    /// </summary>
    public class UserProfileDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// معرف المستخدم
        /// </summary>
        public Guid UserId { get; set; }

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
        /// الحصول على العنوان الكامل
        /// </summary>
        public string FullAddress
        {
            get
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
        }

        /// <summary>
        /// الحصول على روابط التواصل الاجتماعي
        /// </summary>
        public Dictionary<string, string> SocialLinks
        {
            get
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
        }

        /// <summary>
        /// هل يوجد صورة للملف الشخصي؟
        /// </summary>
        public bool HasProfileImage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ProfileImageUrl);
            }
        }

        /// <summary>
        /// هل يوجد نبذة عن المستخدم؟
        /// </summary>
        public bool HasBio
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Bio);
            }
        }

        /// <summary>
        /// هل يوجد موقع إلكتروني؟
        /// </summary>
        public bool HasWebsite
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Website);
            }
        }

        /// <summary>
        /// هل يوجد روابط تواصل اجتماعي؟
        /// </summary>
        public bool HasSocialLinks
        {
            get
            {
                return !string.IsNullOrWhiteSpace(LinkedInUrl) ||
                       !string.IsNullOrWhiteSpace(GitHubUrl) ||
                       !string.IsNullOrWhiteSpace(TwitterUrl);
            }
        }

        /// <summary>
        /// الحصول على وقت آخر تحديث
        /// </summary>
        public string LastUpdateTime
        {
            get
            {
                var timeSince = DateTime.UtcNow - LastProfileUpdate;
                
                if (timeSince.TotalDays > 30)
                    return $"محدث منذ {Math.Floor(timeSince.TotalDays)} يوم";
                else if (timeSince.TotalDays > 1)
                    return $"محدث منذ {Math.Floor(timeSince.TotalDays)} أيام";
                else if (timeSince.TotalHours > 1)
                    return $"محدث منذ {Math.Floor(timeSince.TotalHours)} ساعة";
                else
                    return $"محدث منذ {Math.Floor(timeSince.TotalMinutes)} دقيقة";
            }
        }

        /// <summary>
        /// التحقق من صلاحية بيانات الملف الشخصي
        /// </summary>
        public bool IsValid()
        {
            return UserId != Guid.Empty;
        }

        /// <summary>
        /// الحصول على معلومات مختصرة عن الملف الشخصي
        /// </summary>
        public string GetSummary()
        {
            var summary = new List<string>();
            
            if (HasProfileImage)
                summary.Add("يوجد صورة شخصية");
            
            if (HasBio)
                summary.Add("يوجد نبذة شخصية");
            
            if (HasSocialLinks)
                summary.Add("يوجد روابط تواصل اجتماعي");
            
            return summary.Count > 0 ? string.Join("، ", summary) : "لا توجد معلومات إضافية";
        }
    }
}