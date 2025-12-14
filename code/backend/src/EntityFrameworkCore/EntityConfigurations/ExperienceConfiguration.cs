using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CVSystem.Domain.Entities;

namespace CVSystem.EntityFrameworkCore.EntityConfigurations
{
    public class ExperienceConfiguration : IEntityTypeConfiguration<Experience>
    {
        public void Configure(EntityTypeBuilder<Experience> builder)
        {
            builder.ToTable("Experiences", "CV");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CVId)
                .IsRequired();

            builder.Property(x => x.JobTitle)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("المسمى الوظيفي");

            builder.Property(x => x.Company)
                .IsRequired()
                .HasMaxLength(200)
                .HasComment("الشركة");

            builder.Property(x => x.Location)
                .HasMaxLength(100)
                .HasComment("الموقع");

            builder.Property(x => x.StartDate)
                .IsRequired()
                .HasComment("تاريخ البدء");

            builder.Property(x => x.EndDate)
                .HasComment("تاريخ الانتهاء");

            builder.Property(x => x.IsCurrentlyWorking)
                .HasDefaultValue(false)
                .HasComment("هل يعمل حالياً؟");

            builder.Property(x => x.EmploymentType)
                .HasMaxLength(50)
                .HasDefaultValue("دوام كامل")
                .HasComment("نوع التوظيف");

            builder.Property(x => x.Industry)
                .HasMaxLength(100)
                .HasComment("الصناعة");

            builder.Property(x => x.Description)
                .HasMaxLength(1000)
                .HasComment("الوصف");

            builder.Property(x => x.Achievements)
                .HasMaxLength(2000)
                .HasComment("الإنجازات");

            builder.Property(x => x.SkillsUsed)
                .HasMaxLength(1000)
                .HasComment("المهارات المستخدمة");

            builder.Property(x => x.Order)
                .HasDefaultValue(0)
                .HasComment("الترتيب");

            // Indexes
            builder.HasIndex(x => x.CVId)
                .HasDatabaseName("IX_Experiences_CVId");

            builder.HasIndex(x => x.StartDate)
                .HasDatabaseName("IX_Experiences_StartDate");

            builder.HasIndex(x => x.EndDate)
                .HasDatabaseName("IX_Experiences_EndDate");

            builder.HasIndex(x => x.IsCurrentlyWorking)
                .HasDatabaseName("IX_Experiences_IsCurrentlyWorking");

            builder.HasIndex(x => x.Company)
                .HasDatabaseName("IX_Experiences_Company");

            builder.HasIndex(x => x.JobTitle)
                .HasDatabaseName("IX_Experiences_JobTitle");

            builder.HasIndex(x => x.Order)
                .HasDatabaseName("IX_Experiences_Order");

            // Foreign Key
            builder.HasOne<CV>()
                .WithMany()
                .HasForeignKey(x => x.CVId)
                .OnDelete(DeleteBehavior.Cascade);

            // Query Filters
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}