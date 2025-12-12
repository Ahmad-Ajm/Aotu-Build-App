using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using CVSystem.Application.Contracts.Services;
using CVSystem.Application.DTOs;

namespace CVSystem.Http.API.Controllers
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Account")]
    [Route("api/account")]
    [Authorize]
    public class AccountController : AbpController
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// الحصول على بيانات المستخدم الحالي
        /// </summary>
        /// <returns>بيانات المستخدم</returns>
        [HttpGet("profile")]
        public async Task<ActionResult<UserDto>> GetProfileAsync()
        {
            try
            {
                // TODO: الحصول على معرف المستخدم من التوكن
                // حاليًا نستخدم معرف افتراضي للاختبار
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var result = await _userService.GetUserByIdAsync(userId);
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
                    message = "تم الحصول على بيانات المستخدم",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء الحصول على بيانات المستخدم",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// تحديث بيانات المستخدم الحالي
        /// </summary>
        /// <param name="input">بيانات التحديث</param>
        /// <returns>بيانات المستخدم المحدثة</returns>
        [HttpPut("profile")]
        public async Task<ActionResult<UserDto>> UpdateProfileAsync([FromBody] UpdateUserDto input)
        {
            try
            {
                // TODO: الحصول على معرف المستخدم من التوكن
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var result = await _userService.UpdateUserAsync(userId, input);
                return Ok(new
                {
                    success = true,
                    message = "تم تحديث بيانات المستخدم بنجاح",
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
                    errors = new { general = new[] { ex.Message } }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء تحديث بيانات المستخدم",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// الحصول على الملف الشخصي للمستخدم الحالي
        /// </summary>
        /// <returns>الملف الشخصي</returns>
        [HttpGet("user-profile")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfileAsync()
        {
            try
            {
                // TODO: الحصول على معرف المستخدم من التوكن
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var result = await _userService.GetUserProfileAsync(userId);
                if (result == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "لم يتم العثور على الملف الشخصي"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "تم الحصول على الملف الشخصي",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء الحصول على الملف الشخصي",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// تحديث الملف الشخصي للمستخدم الحالي
        /// </summary>
        /// <param name="input">بيانات تحديث الملف الشخصي</param>
        /// <returns>الملف الشخصي المحدث</returns>
        [HttpPut("user-profile")]
        public async Task<ActionResult<UserProfileDto>> UpdateUserProfileAsync([FromBody] UpdateUserProfileDto input)
        {
            try
            {
                // TODO: الحصول على معرف المستخدم من التوكن
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var result = await _userService.UpdateUserProfileAsync(userId, input);
                return Ok(new
                {
                    success = true,
                    message = "تم تحديث الملف الشخصي بنجاح",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء تحديث الملف الشخصي",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// الحصول على جميع المستخدمين (للمسؤولين فقط)
        /// </summary>
        /// <returns>قائمة المستخدمين</returns>
        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsersAsync()
        {
            try
            {
                var result = await _userService.GetAllUsersAsync();
                return Ok(new
                {
                    success = true,
                    message = "تم الحصول على جميع المستخدمين",
                    data = result,
                    count = result.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء الحصول على المستخدمين",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// البحث عن المستخدمين (للمسؤولين فقط)
        /// </summary>
        /// <param name="searchTerm">مصطلح البحث</param>
        /// <returns>قائمة المستخدمين</returns>
        [HttpGet("users/search/{searchTerm}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserDto>>> SearchUsersAsync(string searchTerm)
        {
            try
            {
                var result = await _userService.SearchUsersAsync(searchTerm);
                return Ok(new
                {
                    success = true,
                    message = "تم البحث عن المستخدمين",
                    data = result,
                    count = result.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء البحث عن المستخدمين",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// الحصول على مستخدم بواسطة المعرف (للمسؤولين فقط)
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <returns>بيانات المستخدم</returns>
        [HttpGet("users/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> GetUserByIdAsync(Guid userId)
        {
            try
            {
                var result = await _userService.GetUserByIdAsync(userId);
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
                    message = "تم الحصول على بيانات المستخدم",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء الحصول على بيانات المستخدم",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// تحديث مستخدم بواسطة المعرف (للمسؤولين فقط)
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <param name="input">بيانات التحديث</param>
        /// <returns>بيانات المستخدم المحدثة</returns>
        [HttpPut("users/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> UpdateUserAsync(Guid userId, [FromBody] UpdateUserDto input)
        {
            try
            {
                var result = await _userService.UpdateUserAsync(userId, input);
                return Ok(new
                {
                    success = true,
                    message = "تم تحديث بيانات المستخدم بنجاح",
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
                    errors = new { general = new[] { ex.Message } }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء تحديث بيانات المستخدم",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// حذف مستخدم (للمسؤولين فقط)
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <returns>هل تم الحذف بنجاح؟</returns>
        [HttpDelete("users/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> DeleteUserAsync(Guid userId)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(userId);
                return Ok(new
                {
                    success = result,
                    message = result ? "تم حذف المستخدم بنجاح" : "فشل حذف المستخدم"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء حذف المستخدم",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// تغيير حالة المستخدم (للمسؤولين فقط)
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <returns>الحالة الجديدة</returns>
        [HttpPost("users/{userId}/toggle-status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> ToggleUserStatusAsync(Guid userId)
        {
            try
            {
                var result = await _userService.ToggleUserStatusAsync(userId);
                return Ok(new
                {
                    success = true,
                    data = result,
                    message = result ? "تم تفعيل المستخدم" : "تم إلغاء تفعيل المستخدم"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء تغيير حالة المستخدم",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// الحصول على عدد المستخدمين (للمسؤولين فقط)
        /// </summary>
        /// <returns>عدد المستخدمين</returns>
        [HttpGet("users/count")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> GetUserCountAsync()
        {
            try
            {
                var result = await _userService.GetUserCountAsync();
                return Ok(new
                {
                    success = true,
                    data = result,
                    message = $"إجمالي عدد المستخدمين: {result}"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء الحصول على عدد المستخدمين",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// الحصول على المستخدمين النشطين (للمسؤولين فقط)
        /// </summary>
        /// <returns>قائمة المستخدمين النشطين</returns>
        [HttpGet("users/active")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserDto>>> GetActiveUsersAsync()
        {
            try
            {
                var result = await _userService.GetActiveUsersAsync();
                return Ok(new
                {
                    success = true,
                    message = "تم الحصول على المستخدمين النشطين",
                    data = result,
                    count = result.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء الحصول على المستخدمين النشطين",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// الحصول على المستخدمين غير النشطين (للمسؤولين فقط)
        /// </summary>
        /// <returns>قائمة المستخدمين غير النشطين</returns>
        [HttpGet("users/inactive")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserDto>>> GetInactiveUsersAsync()
        {
            try
            {
                var result = await _userService.GetInactiveUsersAsync();
                return Ok(new
                {
                    success = true,
                    message = "تم الحصول على المستخدمين غير النشطين",
                    data = result,
                    count = result.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء الحصول على المستخدمين غير النشطين",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// الحصول على المستخدمين الذين سجلوا دخولهم مؤخراً (للمسؤولين فقط)
        /// </summary>
        /// <param name="days">عدد الأيام</param>
        /// <returns>قائمة المستخدمين</returns>
        [HttpGet("users/recently-logged-in")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserDto>>> GetRecentlyLoggedInUsersAsync(int days = 7)
        {
            try
            {
                var result = await _userService.GetRecentlyLoggedInUsersAsync(days);
                return Ok(new
                {
                    success = true,
                    message = $"تم الحصول على المستخدمين الذين سجلوا دخولهم خلال آخر {days} أيام",
                    data = result,
                    count = result.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء الحصول على المستخدمين الذين سجلوا دخولهم مؤخراً",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// التحقق من صلاحيات المستخدم
        /// </summary>
        /// <param name="permission">الصلاحية</param>
        /// <returns>هل يملك الصلاحية؟</returns>
        [HttpGet("has-permission/{permission}")]
        public async Task<ActionResult<bool>> HasPermissionAsync(string permission)
        {
            try
            {
                // TODO: الحصول على معرف المستخدم من التوكن
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var result = await _userService.HasPermissionAsync(userId, permission);
                return Ok(new
                {
                    success = true,
                    data = result,
                    message = result ? "المستخدم يملك الصلاحية" : "المستخدم لا يملك الصلاحية"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء التحقق من الصلاحيات",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// الحصول على صلاحيات المستخدم
        /// </summary>
        /// <returns>قائمة الصلاحيات</returns>
        [HttpGet("permissions")]
        public async Task<ActionResult<List<string>>> GetUserPermissionsAsync()
        {
            try
            {
                // TODO: الحصول على معرف المستخدم من التوكن
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var result = await _userService.GetUserPermissionsAsync(userId);
                return Ok(new
                {
                    success = true,
                    message = "تم الحصول على صلاحيات المستخدم",
                    data = result,
                    count = result.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء الحصول على صلاحيات المستخدم",
                    details = ex.Message
                });
            }
        }
    }

    /// <summary>
    /// بيانات تحديث المستخدم
    /// </summary>
    public class UpdateUserDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Nationality { get; set; }
    }

    /// <summary>
    /// بيانات تحديث الملف الشخصي
    /// </summary>
    public class UpdateUserProfileDto
    {
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Bio { get; set; }
    }
}