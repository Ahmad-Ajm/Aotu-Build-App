using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;
using CVSystem.Application.Contracts.Services;
using CVSystem.Application.DTOs;
using CVSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CVSystem.Application.Services
{
    public class CVService : ApplicationService, ICVService
    {
        private readonly IRepository<CV, Guid> _cvRepository;
        private readonly IRepository<ContactInfo, Guid> _contactInfoRepository;
        private readonly IRepository<Education, Guid> _educationRepository;
        private readonly IRepository<Experience, Guid> _experienceRepository;
        private readonly IRepository<Skill, Guid> _skillRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IObjectMapper _objectMapper;

        public CVService(
            IRepository<CV, Guid> cvRepository,
            IRepository<ContactInfo, Guid> contactInfoRepository,
            IRepository<Education, Guid> educationRepository,
            IRepository<Experience, Guid> experienceRepository,
            IRepository<Skill, Guid> skillRepository,
            IGuidGenerator guidGenerator,
            IObjectMapper objectMapper)
        {
            _cvRepository = cvRepository;
            _contactInfoRepository = contactInfoRepository;
            _educationRepository = educationRepository;
            _experienceRepository = experienceRepository;
            _skillRepository = skillRepository;
            _guidGenerator = guidGenerator;
            _objectMapper = objectMapper;
        }

        public async Task<CVDto> CreateCVAsync(CreateCVDto input)
        {
            // التحقق من صحة البيانات
            if (!input.IsValid())
            {
                throw new ArgumentException("بيانات السيرة الذاتية غير صالحة");
            }

            // إنشاء السيرة الذاتية
            var cv = new CV(
                userId: CurrentUser.Id ?? Guid.Empty,
                title: input.Title
            )
            {
                Template = input.Template,
                IsPublic = input.IsPublic,
                PersonalInfo = System.Text.Json.JsonSerializer.Serialize(input.PersonalInfo),
                ContactInfo = System.Text.Json.JsonSerializer.Serialize(input.ContactInfo)
            };

            // حفظ السيرة الذاتية
            cv = await _cvRepository.InsertAsync(cv, autoSave: true);

            // تحويل إلى DTO وإرجاعه
            return _objectMapper.Map<CV, CVDto>(cv);
        }

        public async Task<CVDto> UpdateCVAsync(Guid id, UpdateCVDto input)
        {
            // الحصول على السيرة الذاتية
            var cv = await _cvRepository.GetAsync(id);
            if (cv == null)
            {
                throw new ArgumentException($"السيرة الذاتية غير موجودة: {id}");
            }

            // التحقق من صلاحيات المستخدم
            if (cv.UserId != CurrentUser.Id)
            {
                throw new UnauthorizedAccessException("ليس لديك صلاحية لتعديل هذه السيرة الذاتية");
            }

            // تحديث البيانات
            if (!string.IsNullOrWhiteSpace(input.Title))
            {
                cv.Title = input.Title;
            }

            if (!string.IsNullOrWhiteSpace(input.Template))
            {
                cv.Template = input.Template;
            }

            if (input.PersonalInfo != null)
            {
                cv.PersonalInfo = System.Text.Json.JsonSerializer.Serialize(input.PersonalInfo);
            }

            if (input.WorkExperience != null)
            {
                cv.WorkExperience = System.Text.Json.JsonSerializer.Serialize(input.WorkExperience);
            }

            if (input.Education != null)
            {
                cv.Education = System.Text.Json.JsonSerializer.Serialize(input.Education);
            }

            if (input.Skills != null)
            {
                cv.Skills = System.Text.Json.JsonSerializer.Serialize(input.Skills);
            }

            if (input.ContactInfo != null)
            {
                cv.ContactInfo = System.Text.Json.JsonSerializer.Serialize(input.ContactInfo);
            }

            // تحديث وقت التعديل
            cv.UpdateLastUpdated();

            // حفظ التعديلات
            cv = await _cvRepository.UpdateAsync(cv, autoSave: true);

            return _objectMapper.Map<CV, CVDto>(cv);
        }

        public async Task<CVDto> GetCVAsync(Guid id)
        {
            var cv = await _cvRepository.GetAsync(id);
            if (cv == null)
            {
                throw new ArgumentException($"السيرة الذاتية غير موجودة: {id}");
            }

            // التحقق من الصلاحيات
            if (cv.UserId != CurrentUser.Id && !cv.IsPublic)
            {
                throw new UnauthorizedAccessException("ليس لديك صلاحية للوصول إلى هذه السيرة الذاتية");
            }

            // زيادة عدد المشاهدات إذا كانت عامة
            if (cv.IsPublic)
            {
                cv.IncrementViewCount();
                await _cvRepository.UpdateAsync(cv);
            }

            return _objectMapper.Map<CV, CVDto>(cv);
        }

        public async Task<CVDto> GetCVByShareLinkAsync(string shareLink)
        {
            var cv = await _cvRepository.FirstOrDefaultAsync(c => c.ShareLink == shareLink);
            if (cv == null)
            {
                throw new ArgumentException($"السيرة الذاتية غير موجودة: {shareLink}");
            }

            // التحقق من الصلاحيات
            if (!cv.IsPublic)
            {
                throw new UnauthorizedAccessException("هذه السيرة الذاتية خاصة");
            }

            // زيادة عدد المشاهدات
            cv.IncrementViewCount();
            await _cvRepository.UpdateAsync(cv);

            return _objectMapper.Map<CV, CVDto>(cv);
        }

        public async Task<List<CVDto>> GetUserCVsAsync(Guid userId)
        {
            // التحقق من صلاحيات المستخدم
            if (userId != CurrentUser.Id)
            {
                throw new UnauthorizedAccessException("ليس لديك صلاحية لعرض سير المستخدم الأخرى");
            }

            var cvs = await _cvRepository
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.LastUpdated)
                .ToListAsync();

            return _objectMapper.Map<List<CV>, List<CVDto>>(cvs);
        }

        public async Task DeleteCVAsync(Guid id)
        {
            var cv = await _cvRepository.GetAsync(id);
            if (cv == null)
            {
                throw new ArgumentException($"السيرة الذاتية غير موجودة: {id}");
            }

            // التحقق من صلاحيات المستخدم
            if (cv.UserId != CurrentUser.Id)
            {
                throw new UnauthorizedAccessException("ليس لديك صلاحية لحذف هذه السيرة الذاتية");
            }

            // حذف السيرة الذاتية
            await _cvRepository.DeleteAsync(cv);
        }

        public async Task<CVDto> ToggleCVVisibilityAsync(Guid id)
        {
            var cv = await _cvRepository.GetAsync(id);
            if (cv == null)
            {
                throw new ArgumentException($"السيرة الذاتية غير موجودة: {id}");
            }

            // التحقق من صلاحيات المستخدم
            if (cv.UserId != CurrentUser.Id)
            {
                throw new UnauthorizedAccessException("ليس لديك صلاحية لتعديل هذه السيرة الذاتية");
            }

            // تبديل حالة الرؤية
            cv.ToggleVisibility();
            cv = await _cvRepository.UpdateAsync(cv, autoSave: true);

            return _objectMapper.Map<CV, CVDto>(cv);
        }

        public async Task IncrementCVViewCountAsync(Guid id)
        {
            var cv = await _cvRepository.GetAsync(id);
            if (cv == null)
            {
                throw new ArgumentException($"السيرة الذاتية غير موجودة: {id}");
            }

            // زيادة عدد المشاهدات
            cv.IncrementViewCount();
            await _cvRepository.UpdateAsync(cv);
        }

        public async Task<byte[]> ExportToPdfAsync(Guid id, ExportOptionsDto options)
        {
            var cv = await _cvRepository.GetAsync(id);
            if (cv == null)
            {
                throw new ArgumentException($"السيرة الذاتية غير موجودة: {id}");
            }

            // التحقق من صلاحيات المستخدم
            if (cv.UserId != CurrentUser.Id && !cv.IsPublic)
            {
                throw new UnauthorizedAccessException("ليس لديك صلاحية لتصدير هذه السيرة الذاتية");
            }

            // TODO: تنفيذ تصدير PDF
            // هذا يحتاج إلى مكتبة مثل iTextSharp أو QuestPDF
            throw new NotImplementedException("تصدير PDF غير متوفر حالياً");
        }

        public async Task<byte[]> ExportToWordAsync(Guid id, ExportOptionsDto options)
        {
            var cv = await _cvRepository.GetAsync(id);
            if (cv == null)
            {
                throw new ArgumentException($"السيرة الذاتية غير موجودة: {id}");
            }

            // التحقق من صلاحيات المستخدم
            if (cv.UserId != CurrentUser.Id && !cv.IsPublic)
            {
                throw new UnauthorizedAccessException("ليس لديك صلاحية لتصدير هذه السيرة الذاتية");
            }

            // TODO: تنفيذ تصدير Word
            // هذا يحتاج إلى مكتبة مثل OpenXML
            throw new NotImplementedException("تصدير Word غير متوفر حالياً");
        }

        public async Task<CVDto> DuplicateCVAsync(Guid id, string newTitle)
        {
            var cv = await _cvRepository.GetAsync(id);
            if (cv == null)
            {
                throw new ArgumentException($"السيرة الذاتية غير موجودة: {id}");
            }

            // التحقق من صلاحيات المستخدم
            if (cv.UserId != CurrentUser.Id)
            {
                throw new UnauthorizedAccessException("ليس لديك صلاحية لنسخ هذه السيرة الذاتية");
            }

            // إنشاء نسخة جديدة
            var newCv = new CV(
                userId: cv.UserId,
                title: string.IsNullOrWhiteSpace(newTitle) ? $"{cv.Title} (نسخة)" : newTitle
            )
            {
                PersonalInfo = cv.PersonalInfo,
                WorkExperience = cv.WorkExperience,
                Education = cv.Education,
                Skills = cv.Skills,
                ContactInfo = cv.ContactInfo,
                Template = cv.Template,
                IsPublic = false // النسخة تكون خاصة افتراضياً
            };

            // حفظ النسخة الجديدة
            newCv = await _cvRepository.InsertAsync(newCv, autoSave: true);

            return _objectMapper.Map<CV, CVDto>(newCv);
        }

        public async Task<List<CVDto>> SearchCVsAsync(string searchTerm, Guid? userId = null)
        {
            var query = _cvRepository.AsQueryable();

            // تصفية حسب المستخدم إذا تم تحديده
            if (userId.HasValue)
            {
                query = query.Where(c => c.UserId == userId.Value);
            }
            else
            {
                // إذا لم يتم تحديد مستخدم، فقط السير الذاتية العامة
                query = query.Where(c => c.IsPublic);
            }

            // البحث في العنوان والمحتوى
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(c =>
                    c.Title.ToLower().Contains(searchTerm) ||
                    c.PersonalInfo.ToLower().Contains(searchTerm) ||
                    c.WorkExperience.ToLower().Contains(searchTerm) ||
                    c.Education.ToLower().Contains(searchTerm) ||
                    c.Skills.ToLower().Contains(searchTerm)
                );
            }

            var cvs = await query
                .OrderByDescending(c => c.LastUpdated)
                .ToListAsync();

            return _objectMapper.Map<List<CV>, List<CVDto>>(cvs);
        }

        public async Task<CVStatisticsDto> GetCVStatisticsAsync(Guid id)
        {
            var cv = await _cvRepository.GetAsync(id);
            if (cv == null)
            {
                throw new ArgumentException($"السيرة الذاتية غير موجودة: {id}");
            }

            // التحقق من صلاحيات المستخدم
            if (cv.UserId != CurrentUser.Id)
            {
                throw new UnauthorizedAccessException("ليس لديك صلاحية لعرض إحصائيات هذه السيرة الذاتية");
            }

            return new CVStatisticsDto
            {
                CVId = cv.Id,
                ViewCount = cv.ViewCount,
                LastUpdated = cv.LastUpdated,
                CreatedAt = cv.CreationTime,
                IsPublic = cv.IsPublic
            };
        }

        public async Task<bool> HasAccessToCVAsync(Guid userId, Guid cvId)
        {
            var cv = await _cvRepository.FindAsync(cvId);
            if (cv == null)
            {
                return false;
            }

            // المستخدم لديه صلاحية إذا كان هو المالك أو السيرة الذاتية عامة
            return cv.UserId == userId || cv.IsPublic;
        }

        public async Task UpdateLastUpdatedAsync(Guid id)
        {
            var cv = await _cvRepository.GetAsync(id);
            if (cv == null)
            {
                throw new ArgumentException($"السيرة الذاتية غير موجودة: {id}");
            }

            // التحقق من صلاحيات المستخدم
            if (cv.UserId != CurrentUser.Id)
            {
                throw new UnauthorizedAccessException("ليس لديك صلاحية لتعديل هذه السيرة الذاتية");
            }

            cv.UpdateLastUpdated();
            await _cvRepository.UpdateAsync(cv);
        }

        public async Task<List<PublicCVDto>> GetPublicCVsAsync(int skip = 0, int take = 10)
        {
            var cvs = await _cvRepository
                .Where(c => c.IsPublic)
                .OrderByDescending(c => c.LastUpdated)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return _objectMapper.Map<List<CV>, List<PublicCVDto>>(cvs);
        }

        public async Task<int> GetUserCVCountAsync(Guid userId)
        {
            return await _cvRepository
                .CountAsync(c => c.UserId == userId);
        }
    }

    // DTOs إضافية مطلوبة للخدمة
    public class UpdateCVDto
    {
        public string Title { get; set; }
        public string Template { get; set; }
        public PersonalInfoDto PersonalInfo { get; set; }
        public object WorkExperience { get; set; }
        public object Education { get; set; }
        public object Skills { get; set; }
        public ContactInfoDto ContactInfo { get; set; }
    }

    public class ExportOptionsDto
    {
        public string Template { get; set; } = "professional";
        public string Language { get; set; } = "ar";
        public bool IncludePhoto { get; set; } = true;
        public bool IncludeSensitiveInfo { get; set; } = false;
    }

    public class CVStatisticsDto
    {
        public Guid CVId { get; set; }
        public int ViewCount { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPublic { get; set; }
    }
}