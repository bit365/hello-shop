// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.IdentityService.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelloShop.IdentityService.Infrastructure.EntityConfigurations;

public class PermissionGrantedEntityTypeConfiguration : IEntityTypeConfiguration<PermissionGranted>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<PermissionGranted> builder)
    {
        builder.ToTable("PermissionGranted");

        builder.Property(x => x.Id);
        builder.Property(x => x.PermissionName).HasMaxLength(64);
        builder.Property(x => x.ResourceType).HasMaxLength(16);
        builder.Property(x => x.ResourceId).HasMaxLength(32);

        builder.HasOne<Role>().WithMany().HasForeignKey(x => x.RoleId).IsRequired();

        builder.HasIndex(x => new { x.RoleId, x.PermissionName, x.ResourceType, x.ResourceId }).IsUnique();
    }
}
