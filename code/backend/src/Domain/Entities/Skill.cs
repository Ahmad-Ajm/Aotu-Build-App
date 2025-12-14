using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace CVSystem.Domain.Entities
{
    /// <summary>
    /// كيان المهارات
    /// </summary>
    public class Skill : FullAuditedEntity<Guid>
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
        /// اسم المهارة
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// مستوى المهارة (مبتدئ، متوسط، متقدم، خبير)
        /// </summary>
        public SkillLevel Level { get; set; }

        /// <summary>
        /// عدد سنوات الخبرة
        /// </summary>
        public int? YearsOfExperience { get; set; }

        /// <summary>
        /// فئة المهارة (تقنية، لغوية، شخصية، إدارية)
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// الوصف الإضافي
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// هل المهارة مميزة؟
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// الترتيب في السيرة الذاتية
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// تاريخ آخر استخدام
        /// </summary>
        public DateTime? LastUsed { get; set; }

        /// <summary>
        /// البناء الافتراضي
        /// </summary>
        public Skill()
        {
            Id = Guid.NewGuid();
            Level = SkillLevel.Intermediate;
            Order = 0;
        }

        /// <summary>
        /// بناء مع المعلمات الأساسية
        /// </summary>
        public Skill(Guid cvId, string name) : this()
        {
            CVId = cvId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// التحقق من صحة البيانات
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && CVId != Guid.Empty;
        }

        /// <summary>
        /// الحصول على مستوى المهارة كنص
        /// </summary>
        public string GetLevelText()
        {
            return Level switch
            {
                SkillLevel.Beginner => "مبتدئ",
                SkillLevel.Intermediate => "متوسط",
                SkillLevel.Advanced => "متقدم",
                SkillLevel.Expert => "خبير",
                _ => "غير محدد"
            };
        }

        /// <summary>
        /// الحصول على مستوى المهارة كنسبة مئوية
        /// </summary>
        public int GetLevelPercentage()
        {
            return Level switch
            {
                SkillLevel.Beginner => 25,
                SkillLevel.Intermediate => 50,
                SkillLevel.Advanced => 75,
                SkillLevel.Expert => 100,
                _ => 0
            };
        }

        /// <summary>
        /// الحصول على وصف الخبرة
        /// </summary>
        public string GetExperienceDescription()
        {
            if (YearsOfExperience.HasValue)
            {
                if (YearsOfExperience.Value == 1)
                    return "سنة واحدة";
                else if (YearsOfExperience.Value == 2)
                    return "سنتين";
                else if (YearsOfExperience.Value <= 10)
                    return $"{YearsOfExperience.Value} سنوات";
                else
                    return $"{YearsOfExperience.Value}+ سنة";
            }
            
            return "خبرة عملية";
        }

        /// <summary>
        /// الحصول على ملخص المهارة
        /// </summary>
        public string GetSummary()
        {
            var summary = $"{Name} ({GetLevelText()})";
            
            if (YearsOfExperience.HasValue)
                summary += $" - {GetExperienceDescription()}";
            
            if (!string.IsNullOrWhiteSpace(Category))
                summary += $" [{Category}]";
            
            return summary;
        }

        /// <summary>
        /// تحديث تاريخ آخر استخدام
        /// </summary>
        public void UpdateLastUsed()
        {
            LastUsed = DateTime.UtcNow;
        }

        /// <summary>
        /// الحصول على أيام منذ آخر استخدام
        /// </summary>
        public int? GetDaysSinceLastUsed()
        {
            if (!LastUsed.HasValue)
                return null;
            
            return (int)(DateTime.UtcNow - LastUsed.Value).TotalDays;
        }

        /// <summary>
        /// الحصول على فئة المهارة المناسبة
        /// </summary>
        public static string GetCategoryForSkill(string skillName)
        {
            var technicalKeywords = new[] { "برمجة", "تطوير", "قاعدة بيانات", "شبكات", "أمن", "سيرفر", "API", "Git", "Docker" };
            var languageKeywords = new[] { "لغة", "ترجمة", "تحدث", "كتابة", "إنجليزية", "عربية", "فرنسية" };
            var softKeywords = new[] { "قيادة", "تواصل", "عمل جماعي", "إدارة", "تفكير", "حل مشكلات", "إبداع" };
            
            var lowerSkill = skillName.ToLower();
            
            if (technicalKeywords.Any(k => lowerSkill.Contains(k.ToLower())))
                return "تقنية";
            
            if (languageKeywords.Any(k => lowerSkill.Contains(k.ToLower())))
                return "لغوية";
            
            if (softKeywords.Any(k => lowerSkill.Contains(k.ToLower())))
                return "شخصية";
            
            return "أخرى";
        }
    }

    /// <summary>
    /// مستوى المهارة
    /// </summary>
    public enum SkillLevel
    {
        /// <summary>
        /// مبتدئ
        /// </summary>
        Beginner = 1,
        
        /// <summary>
        /// متوسط
        /// </summary>
        Intermediate = 2,
        
        /// <summary>
        /// متقدم
        /// </summary>
        Advanced = 3,
        
        /// <summary>
        /// خبير
        /// </summary>
        Expert = 4
    }
}