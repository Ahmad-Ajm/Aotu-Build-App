using System;
using System.Text.RegularExpressions;
using Volo.Abp.Domain.Entities.Auditing;

namespace CVSystem.Domain.Entities
{
    /// <summary>
    /// كيان معلومات الاتصال
    /// </summary>
    public class ContactInfo : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// معرف السيرة الذاتية المرتبطة
        /// </summary>
        public Guid CVId { get; set; }

        /// <summary>
        /// السيرة الذاتية المرتبطة
        /// </summary>
        public virtual CV CV { get; set; }

        /// <summary>
        /// الاسم الكامل
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// البريد الإلكتروني
        /// </summary>
        public string Email { get; set; }

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
        /// رابط الموقع الشخصي
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// رابط LinkedIn
        /// </summary>
        public string LinkedIn { get; set; }

        /// <summary>
        /// رابط GitHub
        /// </summary>
        public string GitHub { get; set; }

        /// <summary>
        /// رابط Twitter/X
        /// </summary>
        public string Twitter { get; set; }

        /// <summary>
        /// رابط موقع آخر
        /// </summary>
        public string OtherWebsite { get; set; }

        /// <summary>
        /// تاريخ الميلاد
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// الجنسية
        /// </summary>
        public string Nationality { get; set; }

        /// <summary>
        /// حالة الإقامة
        /// </summary>
        public string ResidenceStatus { get; set; }

        /// <summary>
        /// هل يظهر رقم الهاتف؟
        /// </summary>
        public bool ShowPhoneNumber { get; set; } = true;

        /// <summary>
        /// هل يظهر العنوان؟
        /// </summary>
        public bool ShowAddress { get; set; } = true;

        /// <summary>
        /// هل يظهر تاريخ الميلاد؟
        /// </summary>
        public bool ShowDateOfBirth { get; set; } = false;

        /// <summary>
        /// البناء الافتراضي
        /// </summary>
        public ContactInfo()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// بناء مع المعلمات الأساسية
        /// </summary>
        public ContactInfo(Guid cvId, string fullName, string email) : this()
        {
            CVId = cvId;
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }

        /// <summary>
        /// التحقق من صحة البيانات
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(FullName) &&
                   IsValidEmail(Email) &&
                   CVId != Guid.Empty;
        }

        /// <summary>
        /// التحقق من صحة البريد الإلكتروني
        /// </summary>
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// التحقق من صحة رقم الهاتف
        /// </summary>
        public bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return true; // الهاتف اختياري

            // نموذج بسيط للتحقق من أرقام الهواتف الدولية
            var regex = new Regex(@"^\+?[1-9]\d{1,14}$");
            return regex.IsMatch(phoneNumber.Replace(" ", "").Replace("-", ""));
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
                parts.Add($"الرمز البريدي: {PostalCode}");
            
            return string.Join("، ", parts);
        }

        /// <summary>
        /// الحصول على العمر
        /// </summary>
        public int? GetAge()
        {
            if (!DateOfBirth.HasValue)
                return null;

            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Value.Year;
            
            if (DateOfBirth.Value.Date > today.AddYears(-age))
                age--;

            return age;
        }

        /// <summary>
        /// الحصول على روابط التواصل الاجتماعي
        /// </summary>
        public Dictionary<string, string> GetSocialLinks()
        {
            var links = new Dictionary<string, string>();
            
            if (!string.IsNullOrWhiteSpace(LinkedIn))
                links.Add("LinkedIn", LinkedIn);
            
            if (!string.IsNullOrWhiteSpace(GitHub))
                links.Add("GitHub", GitHub);
            
            if (!string.IsNullOrWhiteSpace(Twitter))
                links.Add("Twitter", Twitter);
            
            if (!string.IsNullOrWhiteSpace(Website))
                links.Add("الموقع الشخصي", Website);
            
            if (!string.IsNullOrWhiteSpace(OtherWebsite))
                links.Add("موقع آخر", OtherWebsite);
            
            return links;
        }

        /// <summary>
        /// الحصول على معلومات الاتصال الأساسية
        /// </summary>
        public Dictionary<string, string> GetBasicContactInfo()
        {
            var info = new Dictionary<string, string>
            {
                ["الاسم"] = FullName,
                ["البريد الإلكتروني"] = Email
            };

            if (ShowPhoneNumber && !string.IsNullOrWhiteSpace(PhoneNumber))
                info["الهاتف"] = PhoneNumber;

            if (ShowAddress && !string.IsNullOrWhiteSpace(GetFullAddress()))
                info["العنوان"] = GetFullAddress();

            if (ShowDateOfBirth && DateOfBirth.HasValue)
            {
                info["تاريخ الميلاد"] = DateOfBirth.Value.ToString("yyyy-MM-dd");
                var age = GetAge();
                if (age.HasValue)
                    info["العمر"] = age.Value.ToString();
            }

            if (!string.IsNullOrWhiteSpace(Nationality))
                info["الجنسية"] = Nationality;

            return info;
        }

        /// <summary>
        /// تنسيق رقم الهاتف
        /// </summary>
        public string FormatPhoneNumber()
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber))
                return string.Empty;

            var cleaned = PhoneNumber.Replace(" ", "").Replace("-", "").Replace("+", "");
            
            if (cleaned.StartsWith("966")) // السعودية
            {
                if (cleaned.Length == 12) // +9665XXXXXXXX
                    return $"+{cleaned.Substring(0, 3)} {cleaned.Substring(3, 1)} {cleaned.Substring(4, 3)} {cleaned.Substring(7, 4)}";
            }
            else if (cleaned.Length == 10) // أرقام محلية
            {
                return $"{cleaned.Substring(0, 3)}-{cleaned.Substring(3, 3)}-{cleaned.Substring(6, 4)}";
            }

            return PhoneNumber; // إرجاع الرقم كما هو إذا لم نتمكن من تنسيقه
        }

        /// <summary>
        /// إخفاء جزء من البريد الإلكتروني
        /// </summary>
        public string GetMaskedEmail()
        {
            if (string.IsNullOrWhiteSpace(Email))
                return string.Empty;

            var parts = Email.Split('@');
            if (parts.Length != 2)
                return Email;

            var username = parts[0];
            var domain = parts[1];
            
            if (username.Length <= 2)
                return $"{username}@{domain}";
            
            var maskedUsername = username[0] + new string('*', username.Length - 2) + username[^1];
            return $"{maskedUsername}@{domain}";
        }

        /// <summary>
        /// إخفاء جزء من رقم الهاتف
        /// </summary>
        public string GetMaskedPhoneNumber()
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber))
                return string.Empty;

            if (PhoneNumber.Length <= 4)
                return new string('*', PhoneNumber.Length);

            var visibleDigits = 4;
            var maskedPart = new string('*', PhoneNumber.Length - visibleDigits);
            var visiblePart = PhoneNumber.Substring(PhoneNumber.Length - visibleDigits);
            
            return maskedPart + visiblePart;
        }
    }
}