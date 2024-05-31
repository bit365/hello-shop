// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ProductService.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloShop.ProductService.EntityFrameworks.EntityConfigurations.Products;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.Property(x => x.Name).HasMaxLength(32);

        builder.Property(x => x.ImageUrl).HasMaxLength(256);

        builder.HasOne(x => x.Brand).WithMany();
    }
}
