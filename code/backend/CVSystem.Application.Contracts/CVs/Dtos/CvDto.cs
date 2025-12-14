using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace CVSystem.CVs.Dtos
{
    public class CvDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public PersonalInfoForCvDto PersonalInfo { get; set; }
        public List<WorkExperienceForCvDto> WorkExperience { get; set; }
        public List<EducationForCvDto> Education { get; set; }
        public List<SkillForCvDto> Skills { get; set; }
        public string Template { get; set; }
        public bool IsPublic { get; set; }
    }
}
