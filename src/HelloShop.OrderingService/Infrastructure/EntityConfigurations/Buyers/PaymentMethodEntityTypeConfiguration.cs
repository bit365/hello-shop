// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.OrderingService.Entities.Buyers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloShop.OrderingService.Infrastructure.EntityConfigurations.Buyers
{
    public class PaymentMethodEntityTypeConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.ToTable("PaymentMethods");

            builder.Property(x => x.Alias).HasMaxLength(16);
            builder.Property(x => x.CardNumber).HasMaxLength(16);
            builder.Property(x => x.CardHolderName).HasMaxLength(16);
            builder.Property(x => x.SecurityNumber).HasMaxLength(6);
        }
    }
}
