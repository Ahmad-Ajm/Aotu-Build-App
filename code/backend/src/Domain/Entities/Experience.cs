using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace CVSystem.Domain.Entities
{
    /// <summary>
    /// كيان الخبرات العملية
    /// </summary>
    public class Experience : FullAuditedEntity<Guid>
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
        /// المسمى الوظيفي
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// اسم الشركة أو المؤسسة
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// الموقع الجغرافي
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// تاريخ البدء
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// تاريخ الانتهاء
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// هل العمل مستمر؟
        /// </summary>
        public bool IsCurrentlyWorking { get; set; }

        /// <summary>
        /// نوع التوظيف (دوام كامل، جزئي، تعاقد، تدريب)
        /// </summary>
        public string EmploymentType { get; set; }

        /// <summary>
        /// القطاع أو الصناعة
        /// </summary>
        public string Industry { get; set; }

        /// <summary>
        /// الوصف الوظيفي
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// الإنجازات والمهام الرئيسية
        /// </summary>
        public string Achievements { get; set; }

        /// <summary>
        /// المهارات المستخدمة في هذه الوظيفة
        /// </summary>
        public string SkillsUsed { get; set; }

        /// <summary>
        /// الترتيب في السيرة الذاتية
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// البناء الافتراضي
        /// </summary>
        public Experience()
        {
            Id = Guid.NewGuid();
            EmploymentType = "دوام كامل";
            Order = 0;
        }

        /// <summary>
        /// بناء مع المعلمات الأساسية
        /// </summary>
        public Experience(Guid cvId, string jobTitle, string company) : this()
        {
            CVId = cvId;
            JobTitle = jobTitle ?? throw new ArgumentNullException(nameof(jobTitle));
            Company = company ?? throw new ArgumentNullException(nameof(company));
        }

        /// <summary>
        /// حساب مدة العمل
        /// </summary>
        public string GetDuration()
        {
            var endDate = IsCurrentlyWorking ? DateTime.Now : EndDate ?? DateTime.Now;
            
            if (StartDate > endDate)
                return "تاريخ غير صحيح";

            var months = (endDate.Year - StartDate.Year) * 12 + endDate.Month - StartDate.Month;
            
            if (months < 12)
                return $"{months} شهر";
            
            var years = months / 12;
            var remainingMonths = months % 12;
            
            if (remainingMonths == 0)
                return $"{years} سنة";
            
            return $"{years} سنة و {remainingMonths} شهر";
        }

        /// <summary>
        /// الحصول على تاريخ الانتهاء كسلسلة
        /// </summary>
        public string GetEndDateString()
        {
            if (IsCurrentlyWorking)
                return "حتى الآن";
            
            return EndDate?.ToString("yyyy-MM") ?? "غير محدد";
        }

        /// <summary>
        /// الحصول على تاريخ البدء كسلسلة
        /// </summary>
        public string GetStartDateString()
        {
            return StartDate.ToString("yyyy-MM");
        }

        /// <summary>
        /// الحصول على الفترة الزمنية كاملة
        /// </summary>
        public string GetDateRange()
        {
            return $"{GetStartDateString()} - {GetEndDateString()}";
        }

        /// <summary>
        /// التحقق من صحة البيانات
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(JobTitle) &&
                   !string.IsNullOrWhiteSpace(Company) &&
                   StartDate != default &&
                   CVId != Guid.Empty;
        }

        /// <summary>
        /// تحديث حالة العمل الحالية
        /// </summary>
        public void UpdateCurrentWorkStatus()
        {
            if (EndDate.HasValue && EndDate.Value < DateTime.Now)
            {
                IsCurrentlyWorking = false;
            }
        }

        /// <summary>
        /// الحصول على ملخص الخبرة
        /// </summary>
        public string GetSummary(int maxLength = 150)
        {
            var summary = $"{JobTitle} في {Company}";
            
            if (!string.IsNullOrWhiteSpace(Location))
                summary += $" ({Location})";
            
            summary += $" - {GetDateRange()}";
            
            if (!string.IsNullOrWhiteSpace(Description))
            {
                var desc = Description.Length > maxLength ? 
                    Description.Substring(0, maxLength) + "..." : 
                    Description;
                summary += $"\n{desc}";
            }
            
            return summary;
        }

        /// <summary>
        /// تقسيم الإنجازات إلى قائمة
        /// </summary>
        public string[] GetAchievementsList()
        {
            if (string.IsNullOrWhiteSpace(Achievements))
                return Array.Empty<string>();

            return Achievements.Split(new[] { '\n', ';', '•', '-' }, StringSplitOptions.RemoveEmptyEntries)
                              .Select(a => a.Trim())
                              .Where(a => !string.IsNullOrWhiteSpace(a))
                              .ToArray();
        }

        /// <summary>
        /// تقسيم المهارات إلى قائمة
        /// </summary>
        public string[] GetSkillsUsedList()
        {
            if (string.IsNullOrWhiteSpace(SkillsUsed))
                return Array.Empty<string>();

            return SkillsUsed.Split(new[] { ',', ';', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim())
                            .Where(s => !string.IsNullOrWhiteSpace(s))
                            .ToArray();
        }
    }
}