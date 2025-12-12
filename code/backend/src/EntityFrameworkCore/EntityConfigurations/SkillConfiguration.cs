using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CVSystem.Domain.Entities;

namespace CVSystem.EntityFrameworkCore.EntityConfigurations
{
    public class SkillConfiguration : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.ToTable("Skills", "CV");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CVId)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("اسم المهارة");

            builder.Property(x => x.Level)
                .HasDefaultValue(SkillLevel.Intermediate)
                .HasComment("المستوى");

            builder.Property(x => x.YearsOfExperience)
                .HasComment("سنوات الخبرة");

            builder.Property(x => x.Category)
                .HasMaxLength(50)
                .HasComment("الفئة");

            builder.Property(x => x.Description)
                .HasMaxLength(500)
                .HasComment("الوصف");

            builder.Property(x => x.IsFeatured)
                .HasDefaultValue(false)
                .HasComment("مميزة؟");

            builder.Property(x => x.Order)
                .HasDefaultValue(0)
                .HasComment("الترتيب");

            builder.Property(x => x.LastUsed)
                .HasComment("آخر استخدام");

            // Indexes
            builder.HasIndex(x => x.CVId)
                .HasDatabaseName("IX_Skills_CVId");

            builder.HasIndex(x => x.Name)
                .HasDatabaseName("IX_Skills_Name");

            builder.HasIndex(x => x.Level)
                .HasDatabaseName("IX_Skills_Level");

            builder.HasIndex(x => x.Category)
                .HasDatabaseName("IX_Skills_Category");

            builder.HasIndex(x => x.IsFeatured)
                .HasDatabaseName("IX_Skills_IsFeatured");

            builder.HasIndex(x => x.Order)
                .HasDatabaseName("IX_Skills_Order");

            builder.HasIndex(x => x.LastUsed)
                .HasDatabaseName("IX_Skills_LastUsed");

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