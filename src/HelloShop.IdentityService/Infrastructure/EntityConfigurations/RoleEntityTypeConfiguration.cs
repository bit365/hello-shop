// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.IdentityService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloShop.IdentityService.Infrastructure.EntityConfigurations
{
    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("role");

            builder.Property(r => r.Id).HasColumnOrder(1);
            builder.Property(r => r.Name).HasMaxLength(16).HasColumnOrder(2);
            builder.Property(r => r.NormalizedName).HasMaxLength(16).HasColumnOrder(3);
            builder.Property(r => r.ConcurrencyStamp).HasMaxLength(64).HasColumnOrder(4);
            builder.Property(r => r.CreationTime).HasColumnOrder(5);
        }
    }
}
