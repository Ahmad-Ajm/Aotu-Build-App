using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace PsecKit.Core.Domain.Entities
{
    public class User : FullAuditedAggregateRoot<Guid>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? LastLoginDate { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<CV> CVs { get; set; }

        public User()
        {
            IsActive = true;
            FailedLoginAttempts = 0;
            CVs = new HashSet<CV>();
        }
    }
}
