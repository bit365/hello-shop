// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiTenancySample.SchemaIsolationService.Entities;
using MultiTenancySample.SchemaIsolationService.EntityFrameworks;

namespace MultiTenancySample.SchemaIsolationService.Controllers
{
    [Route("api/{tenant}/[controller]")]
    [ApiController]
    public class ProductsController(IDbContextFactory<SchemaIsolationDbContext> dbContextFactory) : ControllerBase
    {
        private readonly SchemaIsolationDbContext _dbContext = dbContextFactory.CreateDbContext();

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
