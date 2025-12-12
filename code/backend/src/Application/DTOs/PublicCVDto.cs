using System;
using Volo.Abp.Application.Dtos;

namespace CVSystem.Application.DTOs
{
    public class PublicCVDto : EntityDto<Guid>
    {
        public string Title { get; set; }
        public string PersonalInfo { get; set; } // Filtered to exclude sensitive data
        public string WorkExperience { get; set; }
        public string Education { get; set; }
        public string Skills { get; set; }
        public string Template { get; set; }
    }
}
