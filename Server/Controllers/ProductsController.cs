using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/products")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly jwtContext _context;
        public ProductsController(jwtContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll(bool? inStock, int? skip, int? take)
        {
            var products = _context.Products.AsQueryable();

            if (inStock != null)
            {
                products = _context.Products.Where(i => i.AvailableQuantity > 0);
            }

            if (skip != null)
            {
                products = _context.Products.Skip((int)skip);
            }

            if (take != null)
            {
                products = _context.Products.Take((int)take);
            }

            return await products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var prod = await _context.Products.FindAsync(id);
            if (prod == null) return NotFound();

            return prod;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Product product)
        {
            if (id != product.ProductId) return BadRequest();

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExist(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = product.ProductId }, product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var prod = await _context.Products.FindAsync(id);
            if (prod == null) return NotFound();

            _context.Products.Remove(prod);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExist(int id)
        {
            return _context.Products.Any(item => item.ProductId == id);
        }
    }
}
