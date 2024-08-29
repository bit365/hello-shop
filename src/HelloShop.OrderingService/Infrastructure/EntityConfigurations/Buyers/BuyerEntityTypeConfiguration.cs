// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.OrderingService.Entities.Buyers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloShop.OrderingService.Infrastructure.EntityConfigurations.Buyers
{
    public class BuyerEntityTypeConfiguration : IEntityTypeConfiguration<Buyer>
    {
        public void Configure(EntityTypeBuilder<Buyer> builder)
        {
            builder.ToTable("Buyer");
            builder.Property(x => x.Name).HasMaxLength(16).IsRequired();
            builder.HasMany(b => b.PaymentMethods).WithOne().HasForeignKey(x => x.BuyerId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
