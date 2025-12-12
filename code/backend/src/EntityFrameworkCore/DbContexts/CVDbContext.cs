using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using CVSystem.Domain.Entities;

namespace CVSystem.EntityFrameworkCore.DbContexts
{
    public class CVDbContext : AbpDbContext<CVDbContext>
    {
        public DbSet<CV> CVs { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Skill> Skills { get; set; }

        public CVDbContext(DbContextOptions<CVDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Configure your own tables/entities inside here */

            builder.Entity<CV>(b =>
            {
                b.ToTable("CVs", "CV");

                b.ConfigureByConvention();

                // Properties
                b.Property(x => x.UserId).IsRequired();
                b.Property(x => x.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                
                b.Property(x => x.PersonalInfo)
                    .HasColumnType("jsonb");
                
                b.Property(x => x.WorkExperience)
                    .HasColumnType("jsonb");
                
                b.Property(x => x.Education)
                    .HasColumnType("jsonb");
                
                b.Property(x => x.Skills)
                    .HasColumnType("jsonb");
                
                b.Property(x => x.ContactInfo)
                    .HasColumnType("jsonb");
                
                b.Property(x => x.Template)
                    .HasMaxLength(50)
                    .HasDefaultValue("professional");
                
                b.Property(x => x.IsPublic)
                    .HasDefaultValue(false);
                
                b.Property(x => x.ShareLink)
                    .HasMaxLength(100);
                
                b.Property(x => x.LastUpdated)
                    .IsRequired();
                
                b.Property(x => x.ViewCount)
                    .HasDefaultValue(0);

                // Indexes
                b.HasIndex(x => x.UserId);
                b.HasIndex(x => x.IsPublic);
                b.HasIndex(x => x.ShareLink).IsUnique();
                b.HasIndex(x => x.LastUpdated);
                b.HasIndex(x => new { x.UserId, x.Title }).IsUnique();
            });

            builder.Entity<ContactInfo>(b =>
            {
                b.ToTable("ContactInfos", "CV");

                b.ConfigureByConvention();

                // Properties
                b.Property(x => x.CVId).IsRequired();
                b.Property(x => x.FullName)
                    .IsRequired()
                    .HasMaxLength(100);
                
                b.Property(x => x.Email)
                    .IsRequired()
                    .HasMaxLength(100);
                
                b.Property(x => x.PhoneNumber)
                    .HasMaxLength(20);
                
                b.Property(x => x.Address)
                    .HasMaxLength(200);
                
                b.Property(x => x.City)
                    .HasMaxLength(50);
                
                b.Property(x => x.Country)
                    .HasMaxLength(50);
                
                b.Property(x => x.PostalCode)
                    .HasMaxLength(20);
                
                b.Property(x => x.Website)
                    .HasMaxLength(200);
                
                b.Property(x => x.LinkedIn)
                    .HasMaxLength(200);
                
                b.Property(x => x.GitHub)
                    .HasMaxLength(200);
                
                b.Property(x => x.Twitter)
                    .HasMaxLength(200);
                
                b.Property(x => x.DateOfBirth);
                
                b.Property(x => x.Nationality)
                    .HasMaxLength(50);
                
                b.Property(x => x.ResidenceStatus)
                    .HasMaxLength(50);
                
                b.Property(x => x.ShowPhoneNumber)
                    .HasDefaultValue(true);
                
                b.Property(x => x.ShowAddress)
                    .HasDefaultValue(true);
                
                b.Property(x => x.ShowDateOfBirth)
                    .HasDefaultValue(false);

                // Indexes
                b.HasIndex(x => x.CVId).IsUnique();
                b.HasIndex(x => x.Email);
                b.HasIndex(x => x.PhoneNumber);
                b.HasIndex(x => x.FullName);

                // Foreign Key
                b.HasOne<CV>()
                    .WithOne()
                    .HasForeignKey<ContactInfo>(x => x.CVId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Education>(b =>
            {
                b.ToTable("Educations", "CV");

                b.ConfigureByConvention();

                // Properties
                b.Property(x => x.CVId).IsRequired();
                b.Property(x => x.Degree)
                    .IsRequired()
                    .HasMaxLength(100);
                
                b.Property(x => x.Institution)
                    .IsRequired()
                    .HasMaxLength(200);
                
                b.Property(x => x.FieldOfStudy)
                    .HasMaxLength(100);
                
                b.Property(x => x.StartDate).IsRequired();
                b.Property(x => x.EndDate);
                b.Property(x => x.IsCurrentlyStudying)
                    .HasDefaultValue(false);
                
                b.Property(x => x.GPA)
                    .HasColumnType("decimal(3,2)");
                
                b.Property(x => x.GPAScale)
                    .HasColumnType("decimal(3,2)")
                    .HasDefaultValue(4.0m);
                
                b.Property(x => x.Description)
                    .HasMaxLength(500);
                
                b.Property(x => x.Location)
                    .HasMaxLength(100);
                
                b.Property(x => x.Order)
                    .HasDefaultValue(0);

                // Indexes
                b.HasIndex(x => x.CVId);
                b.HasIndex(x => x.StartDate);
                b.HasIndex(x => x.EndDate);
                b.HasIndex(x => x.IsCurrentlyStudying);
                b.HasIndex(x => x.Order);

                // Foreign Key
                b.HasOne<CV>()
                    .WithMany()
                    .HasForeignKey(x => x.CVId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Experience>(b =>
            {
                b.ToTable("Experiences", "CV");

                b.ConfigureByConvention();

                // Properties
                b.Property(x => x.CVId).IsRequired();
                b.Property(x => x.JobTitle)
                    .IsRequired()
                    .HasMaxLength(100);
                
                b.Property(x => x.Company)
                    .IsRequired()
                    .HasMaxLength(200);
                
                b.Property(x => x.Location)
                    .HasMaxLength(100);
                
                b.Property(x => x.StartDate).IsRequired();
                b.Property(x => x.EndDate);
                b.Property(x => x.IsCurrentlyWorking)
                    .HasDefaultValue(false);
                
                b.Property(x => x.EmploymentType)
                    .HasMaxLength(50)
                    .HasDefaultValue("دوام كامل");
                
                b.Property(x => x.Industry)
                    .HasMaxLength(100);
                
                b.Property(x => x.Description)
                    .HasMaxLength(1000);
                
                b.Property(x => x.Achievements)
                    .HasMaxLength(2000);
                
                b.Property(x => x.SkillsUsed)
                    .HasMaxLength(1000);
                
                b.Property(x => x.Order)
                    .HasDefaultValue(0);

                // Indexes
                b.HasIndex(x => x.CVId);
                b.HasIndex(x => x.StartDate);
                b.HasIndex(x => x.EndDate);
                b.HasIndex(x => x.IsCurrentlyWorking);
                b.HasIndex(x => x.Company);
                b.HasIndex(x => x.JobTitle);
                b.HasIndex(x => x.Order);

                // Foreign Key
                b.HasOne<CV>()
                    .WithMany()
                    .HasForeignKey(x => x.CVId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Skill>(b =>
            {
                b.ToTable("Skills", "CV");

                b.ConfigureByConvention();

                // Properties
                b.Property(x => x.CVId).IsRequired();
                b.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                
                b.Property(x => x.Level)
                    .HasDefaultValue(SkillLevel.Intermediate);
                
                b.Property(x => x.YearsOfExperience);
                b.Property(x => x.Category)
                    .HasMaxLength(50);
                
                b.Property(x => x.Description)
                    .HasMaxLength(500);
                
                b.Property(x => x.IsFeatured)
                    .HasDefaultValue(false);
                
                b.Property(x => x.Order)
                    .HasDefaultValue(0);
                
                b.Property(x => x.LastUsed);

                // Indexes
                b.HasIndex(x => x.CVId);
                b.HasIndex(x => x.Name);
                b.HasIndex(x => x.Level);
                b.HasIndex(x => x.Category);
                b.HasIndex(x => x.IsFeatured);
                b.HasIndex(x => x.Order);
                b.HasIndex(x => x.LastUsed);

                // Foreign Key
                b.HasOne<CV>()
                    .WithMany()
                    .HasForeignKey(x => x.CVId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}