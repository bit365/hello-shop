// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using AutoMapper;
using HelloShop.ProductService.Entities.Products;
using HelloShop.ProductService.Models.Products;
using HelloShop.ProductService.PermissionProviders;
using HelloShop.ProductService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloShop.ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MockProductsController(IProductService productService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        [Authorize(CatalogPermissions.Products.Default)]
        public async Task<ActionResult<IEnumerable<ProductListItem>>> GetProducts()
        {
            List<Product> products = await productService.GetAllAsync();

            return mapper.Map<List<ProductListItem>>(products);
        }

        [HttpGet("{id}")]
        [Authorize(CatalogPermissions.Products.Details)]
        public async Task<ActionResult<ProductDetailsResponse>> GetProduct(int id)
        {
            Product? entity = await productService.FindAsync(id);

            if (entity is null)
            {
                return NotFound();
            }

            return mapper.Map<ProductDetailsResponse>(entity);
        }

        [HttpPost]
        [Authorize(CatalogPermissions.Products.Create)]
        public async Task<ActionResult<ProductDetailsResponse>> PostProduct(ProductCreateRequest model)
        {
            Product entity = mapper.Map<Product>(model);

            await productService.CreateAsync(entity);

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

            Product? entity = await productService.FindAsync(id);

            if (entity is null)
            {
                return NotFound();
            }

            mapper.Map(model, entity);

            await productService.UpdateAsyc(entity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(CatalogPermissions.Products.Delete)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Product? entity = await productService.FindAsync(id);

            if (entity is null)
            {
                return NotFound();
            }

            await productService.DeleteAsync(entity);

            return NoContent();
        }
    }
}
