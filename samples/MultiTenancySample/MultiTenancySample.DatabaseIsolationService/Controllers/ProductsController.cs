using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiTenancySample.DatabaseIsolationService.Entities;
using MultiTenancySample.DatabaseIsolationService.EntityFrameworks;

namespace MultiTenancySample.DatabaseIsolationService.Controllers
{
    [Route("api/{tenant}/[controller]")]
    [ApiController]
    public class ProductsController(IDbContextFactory<DatabaseIsolationServiceDbContext> dbContextFactory) : ControllerBase
    {
        private readonly DatabaseIsolationServiceDbContext _dbContext = dbContextFactory.CreateDbContext();

        [HttpGet]
        public IActionResult GetProducts(string? keyword)
        {
            IQueryable<Product> products = _dbContext.Set<Product>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                products = products.Where(p => p.Name.Contains(keyword));
            }

            return Ok(products);
        }
    }
}
