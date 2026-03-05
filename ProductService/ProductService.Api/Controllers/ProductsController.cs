using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Api.Data;
using ProductService.Api.Models;

namespace ProductService.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductDbContext _db;

        public ProductsController(ProductDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAll()
            => await _db.Products.AsNoTracking().ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var p = await _db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return p is null ? NotFound() : Ok(p);
        }

        // This helps to validate if product exists by order service
        [HttpGet("{id:int}/exists")]
        public async Task<ActionResult> Exists(int id)
        {
            var exists = await _db.Products.AnyAsync(x => x.Id == id);
            return exists ? Ok() : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, Product updated)
        {
            var p = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (p is null) return NotFound();

            p.Name = updated.Name;
            p.Price = updated.Price;
            p.Stock = updated.Stock;

            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}