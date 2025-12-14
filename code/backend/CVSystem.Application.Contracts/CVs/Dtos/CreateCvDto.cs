using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CVSystem.CVs.Dtos
{
    public class CreateCvDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        public PersonalInfoForCvDto? PersonalInfo { get; set; }
        public List<WorkExperienceForCvDto>? WorkExperience { get; set; }
        public List<EducationForCvDto>? Education { get; set; }
        public List<SkillForCvDto>? Skills { get; set; }
        [StringLength(50)]
        public string Template { get; set; } = "professional";
        public bool IsPublic { get; set; }
    }
}
