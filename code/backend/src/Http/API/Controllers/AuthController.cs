using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using CVSystem.Application.Contracts.Services;
using CVSystem.Application.DTOs;

namespace CVSystem.Http.API.Controllers
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Auth")]
    [Route("api/auth")]
    public class AuthController : AbpController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// تسجيل مستخدم جديد
        /// </summary>
        /// <param name="input">بيانات التسجيل</param>
        /// <returns>استجابة المصادقة</returns>
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> RegisterAsync([FromBody] RegisterDto input)
        {
            try
            {
                var result = await _authService.RegisterAsync(input);
                return Ok(new
                {
                    success = true,
                    message = "تم إنشاء الحساب بنجاح",
                    data = result
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    errors = new { general = new[] { ex.Message } }
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    success = false,
                    message = ex.Message,
                    errors = new { email = new[] { ex.Message } }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء إنشاء الحساب",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// تسجيل الدخول
        /// </summary>
        /// <param name="input">بيانات تسجيل الدخول</param>
        /// <returns>استجابة المصادقة</returns>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> LoginAsync([FromBody] LoginDto input)
        {
            try
            {
                var result = await _authService.LoginAsync(input);
                return Ok(new
                {
                    success = true,
                    message = "تم تسجيل الدخول بنجاح",
                    data = result
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    errors = new { general = new[] { ex.Message } }
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = ex.Message,
                    errors = new { general = new[] { ex.Message } }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء تسجيل الدخول",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// تسجيل الخروج
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <returns>هل تم تسجيل الخروج بنجاح؟</returns>
        [HttpPost("logout")]
        public async Task<ActionResult<bool>> LogoutAsync([FromBody] Guid userId)
        {
            try
            {
                var result = await _authService.LogoutAsync(userId);
                return Ok(new
                {
                    success = result,
                    message = result ? "تم تسجيل الخروج بنجاح" : "فشل تسجيل الخروج"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء تسجيل الخروج",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// تحديث توكن الوصول
        /// </summary>
        /// <param name="refreshToken">توكن التحديث</param>
        /// <returns>استجابة المصادقة الجديدة</returns>
        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDto>> RefreshTokenAsync([FromBody] string refreshToken)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(refreshToken);
                return Ok(new
                {
                    success = true,
                    message = "تم تحديث التوكن بنجاح",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "فشل تحديث التوكن",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// التحقق من صحة التوكن
        /// </summary>
        /// <param name="token">توكن الوصول</param>
        /// <returns>هل التوكن صالح؟</returns>
        [HttpPost("validate-token")]
        public async Task<ActionResult<bool>> ValidateTokenAsync([FromBody] string token)
        {
            try
            {
                var result = await _authService.ValidateTokenAsync(token);
                return Ok(new
                {
                    success = result,
                    message = result ? "التوكن صالح" : "التوكن غير صالح"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "فشل التحقق من صحة التوكن",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// الحصول على المستخدم من التوكن
        /// </summary>
        /// <param name="token">توكن الوصول</param>
        /// <returns>بيانات المستخدم</returns>
        [HttpPost("user-from-token")]
        public async Task<ActionResult<UserDto>> GetUserFromTokenAsync([FromBody] string token)
        {
            try
            {
                var result = await _authService.GetUserFromTokenAsync(token);
                if (result == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "لم يتم العثور على المستخدم"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "تم العثور على المستخدم",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "فشل الحصول على بيانات المستخدم",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// تغيير كلمة المرور
        /// </summary>
        /// <param name="input">بيانات تغيير كلمة المرور</param>
        /// <returns>هل تم تغيير كلمة المرور بنجاح؟</returns>
        [HttpPost("change-password")]
        public async Task<ActionResult<bool>> ChangePasswordAsync([FromBody] ChangePasswordDto input)
        {
            try
            {
                var result = await _authService.ChangePasswordAsync(input.UserId, input.CurrentPassword, input.NewPassword);
                return Ok(new
                {
                    success = result,
                    message = result ? "تم تغيير كلمة المرور بنجاح" : "فشل تغيير كلمة المرور"
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    errors = new { general = new[] { ex.Message } }
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = ex.Message,
                    errors = new { general = new[] { ex.Message } }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء تغيير كلمة المرور",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// نسيان كلمة المرور
        /// </summary>
        /// <param name="email">البريد الإلكتروني</param>
        /// <returns>هل تم إرسال رابط إعادة التعيين؟</returns>
        [HttpPost("forgot-password")]
        public async Task<ActionResult<bool>> ForgotPasswordAsync([FromBody] string email)
        {
            try
            {
                var result = await _authService.ForgotPasswordAsync(email);
                return Ok(new
                {
                    success = result,
                    message = result ? "تم إرسال رابط إعادة تعيين كلمة المرور إلى بريدك الإلكتروني" : "فشل إرسال رابط إعادة التعيين"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء معالجة طلب نسيان كلمة المرور",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// التحقق من صحة توكن إعادة تعيين كلمة المرور
        /// </summary>
        /// <param name="token">توكن إعادة التعيين</param>
        /// <returns>هل التوكن صالح؟</returns>
        [HttpPost("validate-reset-token")]
        public async Task<ActionResult<bool>> ValidateResetPasswordTokenAsync([FromBody] string token)
        {
            try
            {
                var result = await _authService.ValidateResetPasswordTokenAsync(token);
                return Ok(new
                {
                    success = result,
                    message = result ? "توكن إعادة التعيين صالح" : "توكن إعادة التعيين غير صالح"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "فشل التحقق من صحة توكن إعادة التعيين",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// إعادة تعيين كلمة المرور
        /// </summary>
        /// <param name="input">بيانات إعادة التعيين</param>
        /// <returns>هل تم إعادة تعيين كلمة المرور بنجاح؟</returns>
        [HttpPost("reset-password")]
        public async Task<ActionResult<bool>> ResetPasswordAsync([FromBody] ResetPasswordDto input)
        {
            try
            {
                var result = await _authService.ResetPasswordAsync(input.Token, input.NewPassword);
                return Ok(new
                {
                    success = result,
                    message = result ? "تم إعادة تعيين كلمة المرور بنجاح" : "فشل إعادة تعيين كلمة المرور"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "فشل إعادة تعيين كلمة المرور",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// التحقق من وجود مستخدم بالبريد الإلكتروني
        /// </summary>
        /// <param name="email">البريد الإلكتروني</param>
        /// <returns>هل المستخدم موجود؟</returns>
        [HttpGet("user-exists-by-email/{email}")]
        public async Task<ActionResult<bool>> UserExistsByEmailAsync(string email)
        {
            try
            {
                var result = await _authService.UserExistsByEmailAsync(email);
                return Ok(new
                {
                    success = true,
                    data = result,
                    message = result ? "المستخدم موجود" : "المستخدم غير موجود"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء التحقق من وجود المستخدم",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// التحقق من وجود مستخدم باسم المستخدم
        /// </summary>
        /// <param name="userName">اسم المستخدم</param>
        /// <returns>هل المستخدم موجود؟</returns>
        [HttpGet("user-exists-by-username/{userName}")]
        public async Task<ActionResult<bool>> UserExistsByUserNameAsync(string userName)
        {
            try
            {
                var result = await _authService.UserExistsByUserNameAsync(userName);
                return Ok(new
                {
                    success = true,
                    data = result,
                    message = result ? "المستخدم موجود" : "المستخدم غير موجود"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء التحقق من وجود المستخدم",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// الحصول على عدد محاولات الدخول الفاشلة
        /// </summary>
        /// <param name="emailOrUserName">البريد الإلكتروني أو اسم المستخدم</param>
        /// <returns>عدد المحاولات الفاشلة</returns>
        [HttpGet("failed-login-attempts/{emailOrUserName}")]
        public async Task<ActionResult<int>> GetFailedLoginAttemptsAsync(string emailOrUserName)
        {
            try
            {
                var result = await _authService.GetFailedLoginAttemptsAsync(emailOrUserName);
                return Ok(new
                {
                    success = true,
                    data = result,
                    message = $"عدد المحاولات الفاشلة: {result}"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء الحصول على عدد المحاولات الفاشلة",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// التحقق من أن الحساب مقفل
        /// </summary>
        /// <param name="emailOrUserName">البريد الإلكتروني أو اسم المستخدم</param>
        /// <returns>هل الحساب مقفل؟</returns>
        [HttpGet("is-account-locked/{emailOrUserName}")]
        public async Task<ActionResult<bool>> IsAccountLockedAsync(string emailOrUserName)
        {
            try
            {
                var result = await _authService.IsAccountLockedAsync(emailOrUserName);
                return Ok(new
                {
                    success = true,
                    data = result,
                    message = result ? "الحساب مقفل" : "الحساب غير مقفل"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء التحقق من حالة الحساب",
                    details = ex.Message
                });
            }
        }
    }

    /// <summary>
    /// بيانات تغيير كلمة المرور
    /// </summary>
    public class ChangePasswordDto
    {
        public Guid UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    /// <summary>
    /// بيانات إعادة تعيين كلمة المرور
    /// </summary>
    public class ResetPasswordDto
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}