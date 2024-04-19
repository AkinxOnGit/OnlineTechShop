using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq;
using OnlineTechShop.Models;

namespace OnlineTechShop.Controllers;

public class PostShoppingCartProductRequest
{
    public int userId { get; set; }
    public int productId { get; set; }
    public int quantity { get; set; }
}

[Route("api/[controller]")]
[ApiController]
public class ShoppingCartController : ControllerBase
{
    private readonly TechOnlineShopContext _context; // Replace with your DbContext

    public ShoppingCartController(TechOnlineShopContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetShoppingCart(int userId)
    {
        try{
            var shoppingCart = await _context.ShoppingCarts
            .Where(cart => cart.UserId == userId)
            .FirstOrDefaultAsync();

            if (shoppingCart == null)
            {
                return NotFound();
            }

            var shoppingCartProducts = await _context.ShoppingCartProducts
                .Where(shoppingCartProduct => shoppingCartProduct.ShoppingCartId == shoppingCart.ShoppingCartId)
                .ToListAsync();

            var products = new List<Product>();

            foreach (var shoppingCartProduct in shoppingCartProducts)
            {
                var product = await _context.Products
                    .Where(productItem => productItem.ProductId == shoppingCartProduct.ProductId)
                    .FirstOrDefaultAsync();

                if (product != null)
                {
                    products.Add(product);
                }
            }

            return Ok(products);
        } catch (Exception ex){
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<ShoppingCartProduct>> PostShoppingCartItem([FromBody] PostShoppingCartProductRequest options)
    {
        try
        {
            if (options == null)
            {
                return BadRequest("Invalid input data.");
            }

            if (options.userId <= 0 || options.productId <= 0 || options.quantity <= 0)
            {
                return BadRequest("Invalid userId, productId, or quantity.");
            }

            var shoppingCart = await _context.ShoppingCarts.Where(cart => cart.UserId == options.userId).FirstOrDefaultAsync();

            if (shoppingCart != null)
            {
                var shoppingCartProduct = new ShoppingCartProduct
                {
                    ProductId = options.productId,
                    Quantity = options.quantity,
                    ShoppingCartId = shoppingCart.ShoppingCartId,
                };

                _context.ShoppingCartProducts.Add(shoppingCartProduct);
                await _context.SaveChangesAsync();

                return Ok("success");
            }
            else
            {
                return NotFound("Shopping cart not found.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteShoppingCartItem(int userId)
    {
        try {
            var shoppingCart = await _context.ShoppingCarts
                .Where(cart => cart.UserId == userId)
                .FirstOrDefaultAsync();

            if (shoppingCart == null)
            {
                return NotFound();
            }

            var shoppingCartProducts = await _context.ShoppingCartProducts
                .Where(shoppingCartProduct => shoppingCartProduct.ShoppingCartId == shoppingCart.ShoppingCartId)
                .ToListAsync();

            foreach (var shoppingCartProduct in shoppingCartProducts)
            {
                await _context.ShoppingCartProducts.Where(scp => scp.ShoppingCartProductsId == shoppingCartProduct.ShoppingCartProductsId).ExecuteDeleteAsync();
            }
            
            return Ok();
        } catch (Exception ex) {
             return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // [HttpDelete]
    // public async Task<ActionResult<ShoppingCart>> DeleteShoppingCart(int userId)
    // {
    //     var shoppingCart = await _context.ShoppingCarts.Where(cart => cart.UserId == userId).FirstOrDefaultAsync();

    //     if (shoppingCart == null) {
    //         return NotFound();
    //     }
        
    //     return Ok(shoppingCart);
    // }

    // [HttpPost]
    // public async Task<ActionResult<ShoppingCart>> OrderShoppingCart(int userId)
    // {
    //     var shoppingCart = await _context.ShoppingCarts.Where(cart => cart.UserId == userId).FirstOrDefaultAsync();

    //     if (shoppingCart == null) {
    //         return NotFound();
    //     }
        
    //     return Ok(shoppingCart);
    // }
}