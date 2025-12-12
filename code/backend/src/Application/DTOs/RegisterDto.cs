using System;
using System.ComponentModel.DataAnnotations;

namespace CVSystem.Application.DTOs
{
    /// <summary>
    /// DTO لتسجيل مستخدم جديد
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// الاسم الكامل (مطلوب، 2-100 حرف)
        /// </summary>
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "الاسم الكامل يجب أن يكون بين 2 و 100 حرف")]
        public string FullName { get; set; }

        /// <summary>
        /// البريد الإلكتروني (مطلوب، تنسيق صحيح)
        /// </summary>
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "تنسيق البريد الإلكتروني غير صحيح")]
        [StringLength(255, ErrorMessage = "البريد الإلكتروني يجب أن لا يتجاوز 255 حرف")]
        public string Email { get; set; }

        /// <summary>
        /// اسم المستخدم (اختياري، إذا لم يتم تقديمه يستخدم البريد الإلكتروني)
        /// </summary>
        [StringLength(100, ErrorMessage = "اسم المستخدم يجب أن لا يتجاوز 100 حرف")]
        public string UserName { get; set; }

        /// <summary>
        /// كلمة المرور (مطلوب، 8 أحرف على الأقل، تحتوي على حرف كبير وصغير ورقم)
        /// </summary>
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "كلمة المرور يجب أن تكون 8 أحرف على الأقل")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", 
            ErrorMessage = "كلمة المرور يجب أن تحتوي على حرف كبير وصغير ورقم")]
        public string Password { get; set; }

        /// <summary>
        /// تأكيد كلمة المرور (مطابقة لكلمة المرور)
        /// </summary>
        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [Compare("Password", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقتين")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// تاريخ الميلاد (اختياري)
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// الجنسية (اختياري)
        /// </summary>
        [StringLength(100, ErrorMessage = "الجنسية يجب أن لا تتجاوز 100 حرف")]
        public string Nationality { get; set; }

        /// <summary>
        /// التحقق من صحة البيانات الإضافية
        /// </summary>
        public bool IsValid()
        {
            // إذا لم يتم تقديم اسم المستخدم، استخدم البريد الإلكتروني
            if (string.IsNullOrWhiteSpace(UserName))
            {
                UserName = Email;
            }

            return !string.IsNullOrWhiteSpace(FullName) &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !string.IsNullOrWhiteSpace(ConfirmPassword) &&
                   Password == ConfirmPassword;
        }

        /// <summary>
        /// الحصول على رسائل التحقق من الصحة
        /// </summary>
        public List<string> GetValidationErrors()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(FullName))
                errors.Add("الاسم الكامل مطلوب");
            else if (FullName.Length < 2 || FullName.Length > 100)
                errors.Add("الاسم الكامل يجب أن يكون بين 2 و 100 حرف");

            if (string.IsNullOrWhiteSpace(Email))
                errors.Add("البريد الإلكتروني مطلوب");
            else if (!new EmailAddressAttribute().IsValid(Email))
                errors.Add("تنسيق البريد الإلكتروني غير صحيح");

            if (string.IsNullOrWhiteSpace(Password))
                errors.Add("كلمة المرور مطلوبة");
            else if (Password.Length < 8)
                errors.Add("كلمة المرور يجب أن تكون 8 أحرف على الأقل");
            else if (!System.Text.RegularExpressions.Regex.IsMatch(Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$"))
                errors.Add("كلمة المرور يجب أن تحتوي على حرف كبير وصغير ورقم");

            if (string.IsNullOrWhiteSpace(ConfirmPassword))
                errors.Add("تأكيد كلمة المرور مطلوب");
            else if (Password != ConfirmPassword)
                errors.Add("كلمة المرور وتأكيدها غير متطابقتين");

            if (!string.IsNullOrWhiteSpace(UserName) && UserName.Length > 100)
                errors.Add("اسم المستخدم يجب أن لا يتجاوز 100 حرف");

            if (!string.IsNullOrWhiteSpace(Nationality) && Nationality.Length > 100)
                errors.Add("الجنسية يجب أن لا تتجاوز 100 حرف");

            return errors;
        }
    }
}