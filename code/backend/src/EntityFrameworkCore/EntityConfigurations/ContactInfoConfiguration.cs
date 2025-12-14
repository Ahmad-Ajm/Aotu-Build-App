using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CVSystem.Domain.Entities;

namespace CVSystem.EntityFrameworkCore.EntityConfigurations
{
    public class ContactInfoConfiguration : IEntityTypeConfiguration<ContactInfo>
    {
        public void Configure(EntityTypeBuilder<ContactInfo> builder)
        {
            builder.ToTable("ContactInfos", "CV");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CVId)
                .IsRequired();

            builder.Property(x => x.FullName)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("الاسم الكامل");

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("البريد الإلكتروني");

            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(20)
                .HasComment("رقم الهاتف");

            builder.Property(x => x.Address)
                .HasMaxLength(200)
                .HasComment("العنوان");

            builder.Property(x => x.City)
                .HasMaxLength(50)
                .HasComment("المدينة");

            builder.Property(x => x.Country)
                .HasMaxLength(50)
                .HasComment("الدولة");

            builder.Property(x => x.PostalCode)
                .HasMaxLength(20)
                .HasComment("الرمز البريدي");

            builder.Property(x => x.Website)
                .HasMaxLength(200)
                .HasComment("الموقع الإلكتروني");

            builder.Property(x => x.LinkedIn)
                .HasMaxLength(200)
                .HasComment("لينكدإن");

            builder.Property(x => x.GitHub)
                .HasMaxLength(200)
                .HasComment("جيت هاب");

            builder.Property(x => x.Twitter)
                .HasMaxLength(200)
                .HasComment("تويتر");

            builder.Property(x => x.DateOfBirth)
                .HasComment("تاريخ الميلاد");

            builder.Property(x => x.Nationality)
                .HasMaxLength(50)
                .HasComment("الجنسية");

            builder.Property(x => x.ResidenceStatus)
                .HasMaxLength(50)
                .HasComment("حالة الإقامة");

            builder.Property(x => x.ShowPhoneNumber)
                .HasDefaultValue(true)
                .HasComment("عرض رقم الهاتف");

            builder.Property(x => x.ShowAddress)
                .HasDefaultValue(true)
                .HasComment("عرض العنوان");

            builder.Property(x => x.ShowDateOfBirth)
                .HasDefaultValue(false)
                .HasComment("عرض تاريخ الميلاد");

            // Indexes
            builder.HasIndex(x => x.CVId)
                .IsUnique()
                .HasDatabaseName("IX_ContactInfos_CVId");

            builder.HasIndex(x => x.Email)
                .HasDatabaseName("IX_ContactInfos_Email");

            builder.HasIndex(x => x.PhoneNumber)
                .HasDatabaseName("IX_ContactInfos_PhoneNumber");

            builder.HasIndex(x => x.FullName)
                .HasDatabaseName("IX_ContactInfos_FullName");

            // Foreign Key
            builder.HasOne<CV>()
                .WithOne()
                .HasForeignKey<ContactInfo>(x => x.CVId)
                .OnDelete(DeleteBehavior.Cascade);

            // Query Filters
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}