using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace CVSystem.CVs
{
    public class CV : FullAuditedAggregateRoot<Guid>
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string PersonalInfo { get; set; } // JSONB will be used in DB
        public string WorkExperience { get; set; } // JSONB will be used in DB
        public string Education { get; set; } // JSONB will be used in DB
        public string Skills { get; set; } // JSONB will be used in DB
        public string Template { get; set; }
        public bool IsPublic { get; set; }
    }
}
