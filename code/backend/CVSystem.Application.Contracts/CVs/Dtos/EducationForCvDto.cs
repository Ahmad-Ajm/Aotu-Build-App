using System;

namespace CVSystem.CVs.Dtos
{
    public class EducationForCvDto
    {
        public string Degree { get; set; }
        public string Institution { get; set; }
        public string? FieldOfStudy { get; set; }
        public string StartDate { get; set; }
        public string? GraduationDate { get; set; }
        public string? Description { get; set; }
    }
}
