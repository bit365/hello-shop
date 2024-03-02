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

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnOrder(1);

            builder.Property(u => u.UserName).HasMaxLength(50).HasColumnOrder(3);
            builder.Property(u => u.PasswordHash).HasMaxLength(100).HasColumnOrder(2);
        }
    }
}
