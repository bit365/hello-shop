// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloShop.IdentityService.EntityFrameworks.EntityConfigurations
{
    public class UserClaimEntityTypeConfiguration : IEntityTypeConfiguration<IdentityUserClaim<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserClaim<int>> builder)
        {
            builder.ToTable("UserClaims");

            builder.Property(uc => uc.ClaimType).HasMaxLength(128);
        }
    }
}
