using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CVSystem.Domain.Entities;

namespace CVSystem.EntityFrameworkCore.EntityConfigurations
{
    public class CVConfiguration : IEntityTypeConfiguration<CV>
    {
        public void Configure(EntityTypeBuilder<CV> builder)
        {
            builder.ToTable("CVs", "CV");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200)
                .HasComment("عنوان السيرة الذاتية");

            builder.Property(x => x.PersonalInfo)
                .HasColumnType("jsonb")
                .HasComment("معلومات شخصية (JSON)");

            builder.Property(x => x.WorkExperience)
                .HasColumnType("jsonb")
                .HasComment("خبرات عمل (JSON)");

            builder.Property(x => x.Education)
                .HasColumnType("jsonb")
                .HasComment("التعليم (JSON)");

            builder.Property(x => x.Skills)
                .HasColumnType("jsonb")
                .HasComment("المهارات (JSON)");

            builder.Property(x => x.ContactInfo)
                .HasColumnType("jsonb")
                .HasComment("معلومات الاتصال (JSON)");

            builder.Property(x => x.Template)
                .HasMaxLength(50)
                .HasDefaultValue("professional")
                .HasComment("القالب");

            builder.Property(x => x.IsPublic)
                .HasDefaultValue(false)
                .HasComment("حالة الرؤية (عام/خاص)");

            builder.Property(x => x.ShareLink)
                .HasMaxLength(100)
                .HasComment("رابط المشاركة");

            builder.Property(x => x.LastUpdated)
                .IsRequired()
                .HasDefaultValueSql("NOW()")
                .HasComment("آخر تحديث");

            builder.Property(x => x.ViewCount)
                .HasDefaultValue(0)
                .HasComment("عدد المشاهدات");

            // Indexes
            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_CVs_UserId");

            builder.HasIndex(x => x.IsPublic)
                .HasDatabaseName("IX_CVs_IsPublic")
                .HasFilter("IsPublic = true");

            builder.HasIndex(x => x.ShareLink)
                .IsUnique()
                .HasDatabaseName("IX_CVs_ShareLink");

            builder.HasIndex(x => x.LastUpdated)
                .HasDatabaseName("IX_CVs_LastUpdated");

            builder.HasIndex(x => new { x.UserId, x.Title })
                .IsUnique()
                .HasDatabaseName("IX_CVs_UserId_Title");

            // Query Filters
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}