using AutoMapper;
using HelloShop.ProductService.Entities.Products;
using HelloShop.ProductService.EntityFrameworks;
using HelloShop.ProductService.Models.Products;
using HelloShop.ServiceDefaults.Models.Paging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelloShop.ServiceDefaults.Extensions;
using Microsoft.AspNetCore.Authorization;
using HelloShop.ProductService.PermissionProviders;

namespace HelloShop.ProductService;
[Route("api/[controller]")]
[ApiController]
public class BrandsController(ProductServiceDbContext dbContext, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Authorize(CatalogPermissions.Brands.Default)]
    public async Task<ActionResult<PagedResponse<BrandListItem>>> GetBrands([FromQuery] KeywordSearchRequest model)
    {
        IQueryable<Brand> query = dbContext.Set<Brand>().AsNoTracking();

        query = query.WhereIf(model.Keyword is not null, x => model.Keyword != null && x.Name.Contains(model.Keyword));

        var pagedBrands = query.SortAndPageBy(model);

        return new PagedResponse<BrandListItem>(mapper.Map<List<BrandListItem>>(await pagedBrands.ToListAsync()), await query.CountAsync());
    }

    [HttpGet("{id}")]
    [Authorize(CatalogPermissions.Brands.Details)]
    public async Task<ActionResult<BrandDetailsResponse>> GetBrand(int id)
    {
        Brand? entity = await dbContext.Set<Brand>().FindAsync(id);

        if (entity is null)
        {
            return NotFound();
        }

        return mapper.Map<BrandDetailsResponse>(entity);
    }

    [HttpPost]
    [Authorize(CatalogPermissions.Brands.Create)]
    public async Task<ActionResult<BrandDetailsResponse>> PostBrand(BrandCreateRequest model)
    {
        Brand entity = mapper.Map<Brand>(model);

        await dbContext.AddAsync(entity);

        await dbContext.SaveChangesAsync();

        BrandDetailsResponse result = mapper.Map<BrandDetailsResponse>(entity);

        return CreatedAtAction(nameof(GetBrand), new { id = entity.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(CatalogPermissions.Brands.Update)]
    public async Task<IActionResult> PutBrand(int id, BrandUpdateRequest model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        Brand entity = mapper.Map<Brand>(model);

        dbContext.Entry(entity).State = EntityState.Modified;

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!dbContext.Set<Brand>().Any(e => e.Id == id))
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
    [Authorize(CatalogPermissions.Brands.Delete)]
    public async Task<IActionResult> DeleteBrand(int id)
    {
        Brand? entity = await dbContext.Set<Brand>().FindAsync(id);

        if (entity is null)
        {
            return NotFound();
        }

        dbContext.Remove(entity);

        await dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete]
    [Authorize(CatalogPermissions.Brands.Delete)]
    public async Task<IActionResult> DeleteBrands([FromQuery] IEnumerable<int> ids)
    {
        await dbContext.Set<Brand>().Where(e => ids.Contains(e.Id)).ExecuteDeleteAsync();

        return NoContent();
    }
}