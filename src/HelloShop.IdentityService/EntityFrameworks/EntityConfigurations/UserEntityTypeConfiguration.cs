using HelloShop.IdentityService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloShop.IdentityService.EntityFrameworks.EntityConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.Property(u => u.Id).HasColumnOrder(1);
            builder.Property(u => u.UserName).HasMaxLength(16).HasColumnOrder(2);
            builder.Property(u => u.NormalizedUserName).HasMaxLength(16).HasColumnOrder(3);
            builder.Property(u => u.Email).HasMaxLength(32).HasColumnOrder(4);
            builder.Property(u => u.NormalizedEmail).HasMaxLength(32).HasColumnOrder(5);
            builder.Property(u => u.EmailConfirmed).HasColumnOrder(6);
            builder.Property(u => u.PasswordHash).HasMaxLength(512).HasColumnOrder(7);
            builder.Property(u => u.SecurityStamp).HasMaxLength(32).HasColumnOrder(8);
            builder.Property(u => u.ConcurrencyStamp).HasMaxLength(64).HasColumnOrder(9);
            builder.Property(u => u.PhoneNumber).HasMaxLength(16).HasColumnOrder(10);
            builder.Property(u => u.PhoneNumberConfirmed).HasColumnOrder(11);
            builder.Property(u => u.TwoFactorEnabled).HasColumnOrder(12);
            builder.Property(u => u.LockoutEnd).HasColumnOrder(13);
            builder.Property(u => u.LockoutEnabled).HasColumnOrder(14);
            builder.Property(u => u.AccessFailedCount).HasColumnOrder(15);
            builder.Property(u => u.CreationTime).HasColumnOrder(16);
        }
    }
}
