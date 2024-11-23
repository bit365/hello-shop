// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.OrderingService.Entities.Buyers;
using HelloShop.OrderingService.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloShop.OrderingService.Infrastructure.EntityConfigurations.Orders
{
    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.Description).HasMaxLength(64);
            builder.Property(x => x.OrderStatus).HasConversion<string>();

            builder.OwnsOne(x => x.Address, ownedAddress =>
            {
                ownedAddress.Property(x => x.Country).HasColumnName(nameof(Address.Country)).HasMaxLength(8).IsRequired();
                ownedAddress.Property(x => x.State).HasColumnName(nameof(Address.State)).HasMaxLength(16).IsRequired();
                ownedAddress.Property(x => x.City).HasColumnName(nameof(Address.City)).HasMaxLength(16).IsRequired();
                ownedAddress.Property(x => x.Street).HasColumnName(nameof(Address.Street)).HasMaxLength(32).IsRequired();
                ownedAddress.Property(x => x.ZipCode).HasColumnName(nameof(Address.ZipCode)).HasMaxLength(6).IsRequired();
            });

            builder.HasOne<Buyer>().WithMany().HasForeignKey(x => x.BuyerId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.OrderItems).WithOne().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<PaymentMethod>().WithMany().HasForeignKey(x => x.PaymentMethodId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
