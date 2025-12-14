using System;

namespace CVSystem.Application.DTOs
{
    public class UpdateCVDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string PersonalInfo { get; set; }
        public string WorkExperience { get; set; }
        public string Education { get; set; }
        public string Skills { get; set; }
        public string Template { get; set; }
        public bool IsPublic { get; set; }
    }
}
