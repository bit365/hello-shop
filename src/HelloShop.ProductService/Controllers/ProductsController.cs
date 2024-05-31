// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using AutoMapper;
using HelloShop.ProductService.Entities.Products;
using HelloShop.ProductService.EntityFrameworks;
using HelloShop.ProductService.Models.Products;
using HelloShop.ProductService.PermissionProviders;
using HelloShop.ServiceDefaults.Extensions;
using HelloShop.ServiceDefaults.Models.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloShop.ProductService;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(ProductServiceDbContext dbContext, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Authorize(CatalogPermissions.Products.Default)]
    public async Task<ActionResult<PagedResponse<ProductListItem>>> GetProducts([FromQuery] KeywordSearchRequest model)
    {
        IQueryable<Product> query = dbContext.Set<Product>().Include(x => x.Brand).AsNoTracking();

        query = query.WhereIf(model.Keyword is not null, x => model.Keyword != null && x.Name.Contains(model.Keyword));

        var pagedProducts = query.SortAndPageBy(model);

        return new PagedResponse<ProductListItem>(mapper.Map<List<ProductListItem>>(await pagedProducts.ToListAsync()), await query.CountAsync());
    }

    [HttpGet("{id}")]
    [Authorize(CatalogPermissions.Products.Details)]
    public async Task<ActionResult<ProductDetailsResponse>> GetProduct(int id)
    {
        Product? entity = await dbContext.Set<Product>().FindAsync(id);

        if (entity is null)
        {
            return NotFound();
        }

        await dbContext.Entry(entity).Reference(e => e.Brand).LoadAsync();

        return mapper.Map<ProductDetailsResponse>(entity);
    }

    [HttpPost]
    [Authorize(CatalogPermissions.Products.Create)]
    public async Task<ActionResult<ProductDetailsResponse>> PostProduct(ProductCreateRequest model)
    {
        Product entity = mapper.Map<Product>(model);

        await dbContext.AddAsync(entity);

        await dbContext.SaveChangesAsync();

        ProductDetailsResponse result = mapper.Map<ProductDetailsResponse>(entity);

        return CreatedAtAction(nameof(GetProduct), new { id = entity.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(CatalogPermissions.Products.Update)]
    public async Task<IActionResult> PutProduct(int id, ProductUpdateRequest model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        Product entity = mapper.Map<Product>(model);

        dbContext.Entry(entity).State = EntityState.Modified;

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!dbContext.Set<Product>().Any(e => e.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(CatalogPermissions.Products.Delete)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        Product? entity = await dbContext.Set<Product>().FindAsync(id);

        if (entity is null)
        {
            return NotFound();
        }

        dbContext.Remove(entity);

        await dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete]
    [Authorize(CatalogPermissions.Products.Delete)]
    public async Task<IActionResult> DeleteProducts([FromQuery] IEnumerable<int> ids)
    {
        await dbContext.Set<Product>().Where(e => ids.Contains(e.Id)).ExecuteDeleteAsync();

        return NoContent();
    }
}