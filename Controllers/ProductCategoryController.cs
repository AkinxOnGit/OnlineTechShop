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
    [Route("api/[controller]")]
    public class ProductCategoryController : ControllerBase
    {
        private readonly TechOnlineShopContext _context;

        public ProductCategoryController(TechOnlineShopContext context)
        {
            _context = context;
        }
    
        [HttpGet("Categories")]
        public async Task<ActionResult<CategoriesResponse>> GetAllCategories()
        {
            var categories = await _context.Categories.ToListAsync();

            if(categories == null)
            {
                return NotFound("");
            }
                
            CategoriesResponse response = CategoryResponseMapper.MapToCategoriesResponse(categories);

            return Ok(response);
        }
    }
}

public class CategoryResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class CategoriesResponse
{
    public List<CategoryResponse> CategoryList { get; set; }

    public CategoriesResponse()
    {
        CategoryList = new List<CategoryResponse>();
    }
}

public static class CategoryResponseMapper
{

    public static CategoriesResponse MapToCategoriesResponse(List<Category> categories)
    {
        var categoriesSimple = new CategoriesResponse();

        foreach (var category in categories)
        {
            var categorySimple = new CategoryResponse
            {
                Id = category.CategoryId,
                Name = category.CategoryName
            };

            categoriesSimple.CategoryList.Add(categorySimple);
        }

        return categoriesSimple;
    }
}