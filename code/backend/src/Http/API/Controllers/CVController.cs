using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.MultiTenancy;
using CVSystem.Application.Contracts.Services;
using CVSystem.Application.DTOs;
using CVSystem.Application.Services;

namespace CVSystem.Http.API.Controllers
{
    [RemoteService]
    [Area("app")]
    [ControllerName("CV")]
    [Route("api/app/cv")]
    public class CVController : AbpController
    {
        private readonly ICVService _cvService;

        public CVController(ICVService cvService)
        {
            _cvService = cvService;
        }

        /// <summary>
        /// إنشاء سيرة ذاتية جديدة
        /// </summary>
        /// <param name="input">بيانات السيرة الذاتية</param>
        /// <returns>السيرة الذاتية المنشأة</returns>
        [HttpPost]
        public async Task<ActionResult<CVDto>> CreateAsync([FromBody] CreateCVDto input)
        {
            try
            {
                var result = await _cvService.CreateCVAsync(input);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء إنشاء السيرة الذاتية", details = ex.Message });
            }
        }

        /// <summary>
        /// تحديث سيرة ذاتية موجودة
        /// </summary>
        /// <param name="id">معرف السيرة الذاتية</param>
        /// <param name="input">بيانات التحديث</param>
        /// <returns>السيرة الذاتية المحدثة</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<CVDto>> UpdateAsync(Guid id, [FromBody] UpdateCVDto input)
        {
            try
            {
                var result = await _cvService.UpdateCVAsync(id, input);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء تحديث السيرة الذاتية", details = ex.Message });
            }
        }

        /// <summary>
        /// الحصول على سيرة ذاتية
        /// </summary>
        /// <param name="id">معرف السيرة الذاتية</param>
        /// <returns>السيرة الذاتية</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CVDto>> GetAsync(Guid id)
        {
            try
            {
                var result = await _cvService.GetCVAsync(id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء الحصول على السيرة الذاتية", details = ex.Message });
            }
        }

        /// <summary>
        /// الحصول على سيرة ذاتية عبر رابط المشاركة
        /// </summary>
        /// <param name="shareLink">رابط المشاركة</param>
        /// <returns>السيرة الذاتية</returns>
        [HttpGet("share/{shareLink}")]
        public async Task<ActionResult<CVDto>> GetByShareLinkAsync(string shareLink)
        {
            try
            {
                var result = await _cvService.GetCVByShareLinkAsync(shareLink);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء الحصول على السيرة الذاتية", details = ex.Message });
            }
        }

        /// <summary>
        /// الحصول على جميع سير المستخدم الذاتية
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <returns>قائمة السير الذاتية</returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<CVDto>>> GetUserCVsAsync(Guid userId)
        {
            try
            {
                var result = await _cvService.GetUserCVsAsync(userId);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء الحصول على سير المستخدم الذاتية", details = ex.Message });
            }
        }

        /// <summary>
        /// حذف سيرة ذاتية
        /// </summary>
        /// <param name="id">معرف السيرة الذاتية</param>
        /// <returns>نتيجة الحذف</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                await _cvService.DeleteCVAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء حذف السيرة الذاتية", details = ex.Message });
            }
        }

        /// <summary>
        /// تبديل حالة رؤية السيرة الذاتية (عام/خاص)
        /// </summary>
        /// <param name="id">معرف السيرة الذاتية</param>
        /// <returns>السيرة الذاتية المحدثة</returns>
        [HttpPost("{id}/toggle-visibility")]
        public async Task<ActionResult<CVDto>> ToggleVisibilityAsync(Guid id)
        {
            try
            {
                var result = await _cvService.ToggleCVVisibilityAsync(id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء تبديل حالة الرؤية", details = ex.Message });
            }
        }

        /// <summary>
        /// زيادة عدد مشاهدات السيرة الذاتية
        /// </summary>
        /// <param name="id">معرف السيرة الذاتية</param>
        /// <returns>نتيجة العملية</returns>
        [HttpPost("{id}/increment-view")]
        public async Task<IActionResult> IncrementViewAsync(Guid id)
        {
            try
            {
                await _cvService.IncrementCVViewCountAsync(id);
                return Ok(new { message = "تم زيادة عدد المشاهدات بنجاح" });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء زيادة عدد المشاهدات", details = ex.Message });
            }
        }

        /// <summary>
        /// تصدير السيرة الذاتية إلى PDF
        /// </summary>
        /// <param name="id">معرف السيرة الذاتية</param>
        /// <param name="options">خيارات التصدير</param>
        /// <returns>ملف PDF</returns>
        [HttpPost("{id}/export/pdf")]
        public async Task<IActionResult> ExportToPdfAsync(Guid id, [FromBody] ExportOptionsDto options)
        {
            try
            {
                var pdfBytes = await _cvService.ExportToPdfAsync(id, options);
                return File(pdfBytes, "application/pdf", $"CV_{id}.pdf");
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (NotImplementedException ex)
            {
                return StatusCode(501, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء تصدير السيرة الذاتية", details = ex.Message });
            }
        }

        /// <summary>
        /// تصدير السيرة الذاتية إلى Word
        /// </summary>
        /// <param name="id">معرف السيرة الذاتية</param>
        /// <param name="options">خيارات التصدير</param>
        /// <returns>ملف Word</returns>
        [HttpPost("{id}/export/word")]
        public async Task<IActionResult> ExportToWordAsync(Guid id, [FromBody] ExportOptionsDto options)
        {
            try
            {
                var wordBytes = await _cvService.ExportToWordAsync(id, options);
                return File(wordBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"CV_{id}.docx");
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (NotImplementedException ex)
            {
                return StatusCode(501, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء تصدير السيرة الذاتية", details = ex.Message });
            }
        }

        /// <summary>
        /// نسخ سيرة ذاتية
        /// </summary>
        /// <param name="id">معرف السيرة الذاتية الأصلية</param>
        /// <param name="newTitle">عنوان النسخة الجديدة</param>
        /// <returns>النسخة الجديدة</returns>
        [HttpPost("{id}/duplicate")]
        public async Task<ActionResult<CVDto>> DuplicateAsync(Guid id, [FromQuery] string newTitle = null)
        {
            try
            {
                var result = await _cvService.DuplicateCVAsync(id, newTitle);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء نسخ السيرة الذاتية", details = ex.Message });
            }
        }

        /// <summary>
        /// البحث في السير الذاتية
        /// </summary>
        /// <param name="searchTerm">كلمة البحث</param>
        /// <param name="userId">معرف المستخدم (اختياري)</param>
        /// <returns>نتائج البحث</returns>
        [HttpGet("search")]
        public async Task<ActionResult<List<CVDto>>> SearchAsync([FromQuery] string searchTerm, [FromQuery] Guid? userId = null)
        {
            try
            {
                var result = await _cvService.SearchCVsAsync(searchTerm, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء البحث في السير الذاتية", details = ex.Message });
            }
        }

        /// <summary>
        /// الحصول على إحصائيات السيرة الذاتية
        /// </summary>
        /// <param name="id">معرف السيرة الذاتية</param>
        /// <returns>الإحصائيات</returns>
        [HttpGet("{id}/statistics")]
        public async Task<ActionResult<CVStatisticsDto>> GetStatisticsAsync(Guid id)
        {
            try
            {
                var result = await _cvService.GetCVStatisticsAsync(id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء الحصول على الإحصائيات", details = ex.Message });
            }
        }

        /// <summary>
        /// التحقق من صلاحية الوصول إلى سيرة ذاتية
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <param name="cvId">معرف السيرة الذاتية</param>
        /// <returns>هل لديه صلاحية؟</returns>
        [HttpGet("check-access")]
        public async Task<ActionResult<bool>> CheckAccessAsync([FromQuery] Guid userId, [FromQuery] Guid cvId)
        {
            try
            {
                var result = await _cvService.HasAccessToCVAsync(userId, cvId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء التحقق من الصلاحيات", details = ex.Message });
            }
        }

        /// <summary>
        /// تحديث وقت آخر تعديل للسيرة الذاتية
        /// </summary>
        /// <param name="id">معرف السيرة الذاتية</param>
        /// <returns>نتيجة العملية</returns>
        [HttpPost("{id}/update-last-updated")]
        public async Task<IActionResult> UpdateLastUpdatedAsync(Guid id)
        {
            try
            {
                await _cvService.UpdateLastUpdatedAsync(id);
                return Ok(new { message = "تم تحديث وقت آخر تعديل بنجاح" });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء تحديث وقت آخر تعديل", details = ex.Message });
            }
        }

        /// <summary>
        /// الحصول على السير الذاتية العامة
        /// </summary>
        /// <param name="skip">عدد السجلات التي سيتم تخطيها</param>
        /// <param name="take">عدد السجلات المطلوبة</param>
        /// <returns>قائمة السير الذاتية العامة</returns>
        [HttpGet("public")]
        public async Task<ActionResult<List<PublicCVDto>>> GetPublicCVsAsync([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            try
            {
                var result = await _cvService.GetPublicCVsAsync(skip, take);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء الحصول على السير الذاتية العامة", details = ex.Message });
            }
        }

        /// <summary>
        /// الحصول على عدد سير المستخدم الذاتية
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <returns>عدد السير الذاتية</returns>
        [HttpGet("user/{userId}/count")]
        public async Task<ActionResult<int>> GetUserCVCountAsync(Guid userId)
        {
            try
            {
                var result = await _cvService.GetUserCVCountAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء الحصول على عدد السير الذاتية", details = ex.Message });
            }
        }
    }
}