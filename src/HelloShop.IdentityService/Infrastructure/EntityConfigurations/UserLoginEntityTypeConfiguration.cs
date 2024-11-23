// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloShop.IdentityService.Infrastructure.EntityConfigurations
{
    public class UserLoginEntityTypeConfiguration : IEntityTypeConfiguration<IdentityUserLogin<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserLogin<int>> builder)
        {
            builder.ToTable("user_login");

            builder.Property(ul => ul.LoginProvider).HasMaxLength(16);
            builder.Property(ul => ul.ProviderDisplayName).HasMaxLength(16);
        }
    }
}
