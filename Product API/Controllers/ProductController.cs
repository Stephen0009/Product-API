using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product_API.AppDbContext;
using Product_API.Models;

namespace Product_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext context;

        public ProductsController(DataContext _context)
        {
            context = _context;
        }
        [HttpGet]
        //GET:    api/product
        public IEnumerable<Product> Products()
        {
            return context.Products.ToList();
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<Product>> GetProductById(int Id)
        {
            var product = await context.Products.FindAsync(Id);
            if (product != null)
            {
                return product;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product newProduct)
        {
            await context.Products.AddAsync(newProduct);
            return CreatedAtAction("Products",new Product{ ProductId = newProduct.ProductId },newProduct);
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> UpdateProduct(int Id, Product newProduct)
        {
            if(Id != newProduct.ProductId)
            {
                return BadRequest();
            }
            
            var product = await context.Products.FindAsync(Id);
            if (product != null)
            {
                context.Products.Update(newProduct);
                await context.SaveChangesAsync();

                return NoContent();
            }
            return NotFound();
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int Id)
        {
            var product = await context.Products.FindAsync(Id);
            if (product != null)
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return product;
            }
            return NotFound();
        }
    }
}