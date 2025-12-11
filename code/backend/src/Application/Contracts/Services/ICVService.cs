using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CVSystem.Application.DTOs;

namespace CVSystem.Application.Contracts.Services
{
    /// <summary>
    /// واجهة خدمة السير الذاتية
    /// </summary>
    public interface ICVService
    {
        /// <summary>
        /// إنشاء سيرة ذاتية جديدة
        /// </summary>
        Task<CVDto> CreateCVAsync(CreateCVDto input);

        /// <summary>
        /// تحديث سيرة ذاتية موجودة
        /// </summary>
        Task<CVDto> UpdateCVAsync(Guid id, UpdateCVDto input);

        /// <summary>
        /// الحصول على سيرة ذاتية بواسطة المعرف
        /// </summary>
        Task<CVDto> GetCVAsync(Guid id);

        /// <summary>
        /// الحصول على سيرة ذاتية بواسطة رابط المشاركة
        /// </summary>
        Task<CVDto> GetCVByShareLinkAsync(string shareLink);

        /// <summary>
        /// الحصول على جميع السير الذاتية للمستخدم
        /// </summary>
        Task<List<CVDto>> GetUserCVsAsync(Guid userId);

        /// <summary>
        /// حذف سيرة ذاتية
        /// </summary>
        Task DeleteCVAsync(Guid id);

        /// <summary>
        /// تغيير حالة السيرة الذاتية (عامة/خاصة)
        /// </summary>
        Task<CVDto> ToggleCVVisibilityAsync(Guid id);

        /// <summary>
        /// زيادة عدد مشاهدات السيرة الذاتية
        /// </summary>
        Task IncrementCVViewCountAsync(Guid id);

        /// <summary>
        /// تصدير السيرة الذاتية إلى PDF
        /// </summary>
        Task<byte[]> ExportToPdfAsync(Guid id, ExportOptionsDto options);

        /// <summary>
        /// تصدير السيرة الذاتية إلى Word
        /// </summary>
        Task<byte[]> ExportToWordAsync(Guid id, ExportOptionsDto options);

        /// <summary>
        /// نسخ سيرة ذاتية موجودة
        /// </summary>
        Task<CVDto> DuplicateCVAsync(Guid id, string newTitle);

        /// <summary>
        /// البحث في السير الذاتية
        /// </summary>
        Task<List<CVDto>> SearchCVsAsync(string searchTerm, Guid? userId = null);

        /// <summary>
        /// الحصول على إحصائيات السيرة الذاتية
        /// </summary>
        Task<CVStatisticsDto> GetCVStatisticsAsync(Guid id);

        /// <summary>
        /// التحقق من صلاحية الوصول إلى السيرة الذاتية
        /// </summary>
        Task<bool> HasAccessToCVAsync(Guid userId, Guid cvId);

        /// <summary>
        /// تحديث تاريخ آخر تحديث للسيرة الذاتية
        /// </summary>
        Task UpdateLastUpdatedAsync(Guid id);

        /// <summary>
        /// الحصول على السير الذاتية العامة
        /// </summary>
        Task<List<PublicCVDto>> GetPublicCVsAsync(int skip = 0, int take = 10);

        /// <summary>
        /// الحصول على عدد السير الذاتية للمستخدم
        /// </summary>
        Task<int> GetUserCVCountAsync(Guid userId);
    }
}