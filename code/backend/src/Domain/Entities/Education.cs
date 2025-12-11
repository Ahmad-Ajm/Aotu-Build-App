using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace CVSystem.Domain.Entities
{
    /// <summary>
    /// كيان التعليم والمؤهلات
    /// </summary>
    public class Education : FullAuditedEntity<Guid>
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
        /// الدرجة العلمية (مثال: بكالوريوس، ماجستير، دكتوراه)
        /// </summary>
        public string Degree { get; set; }

        /// <summary>
        /// المؤسسة التعليمية
        /// </summary>
        public string Institution { get; set; }

        /// <summary>
        /// التخصص
        /// </summary>
        public string FieldOfStudy { get; set; }

        /// <summary>
        /// تاريخ البدء
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// تاريخ التخرج
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// هل الدراسة مستمرة؟
        /// </summary>
        public bool IsCurrentlyStudying { get; set; }

        /// <summary>
        /// المعدل التراكمي
        /// </summary>
        public decimal? GPA { get; set; }

        /// <summary>
        /// المقياس الأقصى للمعدل (مثال: 4.0، 5.0، 100)
        /// </summary>
        public decimal? GPAScale { get; set; }

        /// <summary>
        /// الوصف الإضافي
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// الموقع الجغرافي
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// الترتيب في السيرة الذاتية
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// البناء الافتراضي
        /// </summary>
        public Education()
        {
            Id = Guid.NewGuid();
            Order = 0;
        }

        /// <summary>
        /// بناء مع المعلمات الأساسية
        /// </summary>
        public Education(Guid cvId, string degree, string institution) : this()
        {
            CVId = cvId;
            Degree = degree ?? throw new ArgumentNullException(nameof(degree));
            Institution = institution ?? throw new ArgumentNullException(nameof(institution));
        }

        /// <summary>
        /// حساب مدة الدراسة
        /// </summary>
        public string GetDuration()
        {
            var endDate = IsCurrentlyStudying ? DateTime.Now : EndDate ?? DateTime.Now;
            
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
        /// الحصول على تاريخ التخرج كسلسلة
        /// </summary>
        public string GetEndDateString()
        {
            if (IsCurrentlyStudying)
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
        /// الحصول على المعدل التراكمي كنسبة مئوية
        /// </summary>
        public string GetGPAPercentage()
        {
            if (!GPA.HasValue || !GPAScale.HasValue || GPAScale.Value == 0)
                return null;

            var percentage = (GPA.Value / GPAScale.Value) * 100;
            return $"{percentage:F1}%";
        }

        /// <summary>
        /// التحقق من صحة البيانات
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Degree) &&
                   !string.IsNullOrWhiteSpace(Institution) &&
                   StartDate != default &&
                   CVId != Guid.Empty;
        }

        /// <summary>
        /// تحديث حالة الدراسة الحالية
        /// </summary>
        public void UpdateCurrentStudyStatus()
        {
            if (EndDate.HasValue && EndDate.Value < DateTime.Now)
            {
                IsCurrentlyStudying = false;
            }
        }
    }
}