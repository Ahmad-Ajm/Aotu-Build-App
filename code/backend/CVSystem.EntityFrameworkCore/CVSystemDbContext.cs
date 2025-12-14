using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using CVSystem.Users;
using CVSystem.CVs;

namespace CVSystem.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
public class CVSystemDbContext :
    AbpDbContext<CVSystemDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots and Entities here. */

    #region Entities from the modules

    /* Inherited DbSets from Storybook modules: */
    public DbSet<CV> CVs { get; set; }
    public DbSet<AppUser> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }

    #endregion

    public CVSystemDbContext(DbContextOptions<CVSystemDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to build your domain model.*/

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        builder.Entity<AppUser>(b =>
        {
            b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "Users"); // Moved back to AbpIdentityDbProperties
            b.ConfigureByConvention();
            b.Property(x => x.CreationTime).HasColumnName(nameof(AppUser.CreationTime));
            b.HasMany(x => x.UserProfiles).WithOne().HasForeignKey(x => x.UserId).IsRequired();
            b.HasIndex(x => x.NormalizedUserName);
            b.HasIndex(x => x.NormalizedEmail);
        });

        builder.Entity<UserProfile>(b =>
        {
            b.ToTable("UserProfiles");
            b.ConfigureByConvention();
            b.HasKey(x => x.Id);
            b.Property(x => x.FullName).IsRequired().HasMaxLength(UserProfileConsts.FullNameMaxLength);
            b.HasOne<AppUser>().WithMany(x => x.UserProfiles).HasForeignKey(x => x.UserId).IsRequired();
            b.HasIndex(x => x.UserId);
        });

        builder.Entity<CV>(b =>
        {
            b.ToTable("CVs");
            b.ConfigureByConvention();
            b.Property(x => x.Title).IsRequired().HasMaxLength(200);
            b.Property(x => x.PersonalInfo).HasColumnType("jsonb");
            b.Property(x => x.WorkExperience).HasColumnType("jsonb");
            b.Property(x => x.Education).HasColumnType("jsonb");
            b.Property(x => x.Skills).HasColumnType("jsonb");
            b.Property(x => x.Template).HasMaxLength(50);
            b.HasIndex(x => x.UserId);
            b.HasIndex(x => x.IsPublic);
        });
    }
}
