using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CVSystem.Domain.Entities;

namespace CVSystem.EntityFrameworkCore.EntityConfigurations
{
    public class EducationConfiguration : IEntityTypeConfiguration<Education>
    {
        public void Configure(EntityTypeBuilder<Education> builder)
        {
            builder.ToTable("Educations", "CV");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CVId)
                .IsRequired();

            builder.Property(x => x.Degree)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("الدرجة العلمية");

            builder.Property(x => x.Institution)
                .IsRequired()
                .HasMaxLength(200)
                .HasComment("المؤسسة التعليمية");

            builder.Property(x => x.FieldOfStudy)
                .HasMaxLength(100)
                .HasComment("مجال الدراسة");

            builder.Property(x => x.StartDate)
                .IsRequired()
                .HasComment("تاريخ البدء");

            builder.Property(x => x.EndDate)
                .HasComment("تاريخ الانتهاء");

            builder.Property(x => x.IsCurrentlyStudying)
                .HasDefaultValue(false)
                .HasComment("هل يدرس حالياً؟");

            builder.Property(x => x.GPA)
                .HasColumnType("decimal(3,2)")
                .HasComment("المعدل التراكمي");

            builder.Property(x => x.GPAScale)
                .HasColumnType("decimal(3,2)")
                .HasDefaultValue(4.0m)
                .HasComment("مقياس المعدل");

            builder.Property(x => x.Description)
                .HasMaxLength(500)
                .HasComment("الوصف");

            builder.Property(x => x.Location)
                .HasMaxLength(100)
                .HasComment("الموقع");

            builder.Property(x => x.Order)
                .HasDefaultValue(0)
                .HasComment("الترتيب");

            // Indexes
            builder.HasIndex(x => x.CVId)
                .HasDatabaseName("IX_Educations_CVId");

            builder.HasIndex(x => x.StartDate)
                .HasDatabaseName("IX_Educations_StartDate");

            builder.HasIndex(x => x.EndDate)
                .HasDatabaseName("IX_Educations_EndDate");

            builder.HasIndex(x => x.IsCurrentlyStudying)
                .HasDatabaseName("IX_Educations_IsCurrentlyStudying");

            builder.HasIndex(x => x.Order)
                .HasDatabaseName("IX_Educations_Order");

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