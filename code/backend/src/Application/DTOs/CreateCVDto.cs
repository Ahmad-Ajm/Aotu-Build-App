using System;
using System.ComponentModel.DataAnnotations;

namespace CVSystem.Application.DTOs
{
    /// <summary>
    /// DTO لإنشاء سيرة ذاتية جديدة
    /// </summary>
    public class CreateCVDto
    {
        /// <summary>
        /// عنوان السيرة الذاتية
        /// </summary>
        [Required(ErrorMessage = "عنوان السيرة الذاتية مطلوب")]
        [StringLength(200, ErrorMessage = "العنوان يجب أن يكون بين 3 و 200 حرف", MinimumLength = 3)]
        public string Title { get; set; }

        /// <summary>
        /// القالب المستخدم
        /// </summary>
        [StringLength(50, ErrorMessage = "اسم القالب يجب أن لا يتجاوز 50 حرف")]
        public string Template { get; set; } = "professional";

        /// <summary>
        /// حالة السيرة الذاتية (عامة/خاصة)
        /// </summary>
        public bool IsPublic { get; set; } = false;

        /// <summary>
        /// المعلومات الشخصية
        /// </summary>
        public PersonalInfoDto PersonalInfo { get; set; }

        /// <summary>
        /// معلومات الاتصال
        /// </summary>
        public ContactInfoDto ContactInfo { get; set; }

        /// <summary>
        /// التحقق من صحة البيانات
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Title) && Title.Length >= 3 && Title.Length <= 200;
        }
    }

    /// <summary>
    /// DTO للمعلومات الشخصية
    /// </summary>
    public class PersonalInfoDto
    {
        /// <summary>
        /// الاسم الكامل
        /// </summary>
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم يجب أن يكون بين 2 و 100 حرف", MinimumLength = 2)]
        public string FullName { get; set; }

        /// <summary>
        /// المسمى الوظيفي
        /// </summary>
        [StringLength(100, ErrorMessage = "المسمى الوظيفي يجب أن لا يتجاوز 100 حرف")]
        public string JobTitle { get; set; }

        /// <summary>
        /// نبذة مختصرة
        /// </summary>
        [StringLength(500, ErrorMessage = "النبذة يجب أن لا تتجاوز 500 حرف")]
        public string Summary { get; set; }

        /// <summary>
        /// صورة شخصية (Base64)
        /// </summary>
        public string ProfileImage { get; set; }
    }

    /// <summary>
    /// DTO لمعلومات الاتصال
    /// </summary>
    public class ContactInfoDto
    {
        /// <summary>
        /// البريد الإلكتروني
        /// </summary>
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [StringLength(100, ErrorMessage = "البريد الإلكتروني يجب أن لا يتجاوز 100 حرف")]
        public string Email { get; set; }

        /// <summary>
        /// رقم الهاتف
        /// </summary>
        [StringLength(20, ErrorMessage = "رقم الهاتف يجب أن لا يتجاوز 20 حرف")]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// العنوان
        /// </summary>
        [StringLength(200, ErrorMessage = "العنوان يجب أن لا يتجاوز 200 حرف")]
        public string Address { get; set; }

        /// <summary>
        /// المدينة
        /// </summary>
        [StringLength(50, ErrorMessage = "المدينة يجب أن لا تتجاوز 50 حرف")]
        public string City { get; set; }

        /// <summary>
        /// الدولة
        /// </summary>
        [StringLength(50, ErrorMessage = "الدولة يجب أن لا تتجاوز 50 حرف")]
        public string Country { get; set; }

        /// <summary>
        /// رابط LinkedIn
        /// </summary>
        [Url(ErrorMessage = "رابط LinkedIn غير صحيح")]
        [StringLength(200, ErrorMessage = "رابط LinkedIn يجب أن لا يتجاوز 200 حرف")]
        public string LinkedIn { get; set; }

        /// <summary>
        /// رابط GitHub
        /// </summary>
        [Url(ErrorMessage = "رابط GitHub غير صحيح")]
        [StringLength(200, ErrorMessage = "رابط GitHub يجب أن لا يتجاوز 200 حرف")]
        public string GitHub { get; set; }

        /// <summary>
        /// رابط موقع شخصي
        /// </summary>
        [Url(ErrorMessage = "رابط الموقع الشخصي غير صحيح")]
        [StringLength(200, ErrorMessage = "رابط الموقع الشخصي يجب أن لا يتجاوز 200 حرف")]
        public string Website { get; set; }
    }
}