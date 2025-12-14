using System;
using System.ComponentModel.DataAnnotations;

namespace CVSystem.CVs.Dtos
{
    public class UpdateCvDto : CreateCvDto
    {
        [Required]
        public Guid Id { get; set; }
    }
}
