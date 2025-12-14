using CVSystem.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CVSystem.EntityFrameworkCore;

public class CVSystemDbContext : DbContext
{
    public CVSystemDbContext(DbContextOptions<CVSystemDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(b =>
        {
            b.ToTable("Users");
            b.HasKey(x => x.Id);
            b.Property(x => x.UserName).HasMaxLength(100).IsRequired();
            b.HasIndex(x => x.UserName).IsUnique();
            b.Property(x => x.Email).HasMaxLength(255).IsRequired();
            b.HasIndex(x => x.Email).IsUnique();
            b.Property(x => x.PasswordHash).HasMaxLength(255).IsRequired();
            b.Property(x => x.FullName).HasMaxLength(100).IsRequired();
            b.Property(x => x.IsActive).HasDefaultValue(true);
            b.Property(x => x.FailedLoginAttempts).HasDefaultValue(0);
            b.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            b.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            b.HasOne(x => x.Profile)
             .WithOne(x => x.User)
             .HasForeignKey<UserProfile>(x => x.UserId);
        });

        modelBuilder.Entity<UserProfile>(b =>
        {
            b.ToTable("UserProfiles");
            b.HasKey(x => x.Id);
            b.Property(x => x.PhoneNumber).HasMaxLength(20);
            b.Property(x => x.ProfileImageUrl).HasMaxLength(500);
            b.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            b.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
    }
}
