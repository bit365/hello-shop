using HelloShop.ProductService.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloShop.ProductService.EntityFrameworks.EntityConfigurations.Products;

    public class BrandEntityTypeConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("Brands");
            builder.Property(x => x.Name).HasMaxLength(32);
        }
    }