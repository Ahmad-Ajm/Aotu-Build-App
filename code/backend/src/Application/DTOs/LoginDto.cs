using System.ComponentModel.DataAnnotations;

namespace CVSystem.Application.DTOs
{
    /// <summary>
    /// DTO لتسجيل الدخول
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// البريد الإلكتروني أو اسم المستخدم
        /// </summary>
        [Required(ErrorMessage = "البريد الإلكتروني أو اسم المستخدم مطلوب")]
        [StringLength(255, ErrorMessage = "البريد الإلكتروني أو اسم المستخدم يجب أن لا يتجاوز 255 حرف")]
        public string EmailOrUserName { get; set; }

        /// <summary>
        /// كلمة المرور
        /// </summary>
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        public string Password { get; set; }

        /// <summary>
        /// هل يريد المستخدم تذكر الدخول؟
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        /// التحقق من صحة البيانات
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(EmailOrUserName) &&
                   !string.IsNullOrWhiteSpace(Password);
        }

        /// <summary>
        /// الحصول على رسائل التحقق من الصحة
        /// </summary>
        public List<string> GetValidationErrors()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(EmailOrUserName))
                errors.Add("البريد الإلكتروني أو اسم المستخدم مطلوب");
            else if (EmailOrUserName.Length > 255)
                errors.Add("البريد الإلكتروني أو اسم المستخدم يجب أن لا يتجاوز 255 حرف");

            if (string.IsNullOrWhiteSpace(Password))
                errors.Add("كلمة المرور مطلوبة");

            return errors;
        }

        /// <summary>
        /// تحديد ما إذا كان الإدخال بريد إلكتروني
        /// </summary>
        public bool IsEmail()
        {
            if (string.IsNullOrWhiteSpace(EmailOrUserName))
                return false;

            return new EmailAddressAttribute().IsValid(EmailOrUserName);
        }

        /// <summary>
        /// الحصول على البريد الإلكتروني (إذا كان الإدخال بريد إلكتروني)
        /// </summary>
        public string GetEmail()
        {
            return IsEmail() ? EmailOrUserName : null;
        }

        /// <summary>
        /// الحصول على اسم المستخدم (إذا لم يكن الإدخال بريد إلكتروني)
        /// </summary>
        public string GetUserName()
        {
            return !IsEmail() ? EmailOrUserName : null;
        }
    }
}