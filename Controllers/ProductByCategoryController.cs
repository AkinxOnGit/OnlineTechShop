using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineTechShop.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OnlineTechShop.Controllers
{
    [ApiController]
    [Route("api/Products")]
    public class ProductByCategoryController : ControllerBase
    {
     private readonly TechOnlineShopContext _context;

     public ProductByCategoryController(TechOnlineShopContext context)  
     {
        _context = context;
     }

    [HttpGet("{category}")]
    public async Task<ActionResult<Product>> GetProductsByCategory(int category)
    {
        var product = await _context.Products.Where(p => p.Category != null && p.Category.CategoryId == category).ToListAsync();
        
        if(product == null)
        {
            return NotFound($"Not found {product}");
        }

        return Ok(product);
        }
    }
}