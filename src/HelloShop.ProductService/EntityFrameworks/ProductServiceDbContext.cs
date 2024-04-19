using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HelloShop.ProductService.EntityFrameworks;

 public class ProductServiceDbContext(DbContextOptions<ProductServiceDbContext> options) : DbContext(options)
 {
     protected override void OnModelCreating(ModelBuilder builder)
     {
         base.OnModelCreating(builder);

         builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
     }
 }
