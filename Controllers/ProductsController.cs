using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineTechShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OnlineTechShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        public class PaginationInfo
        {
            public int page {get; set;}
            public int pageSize {get; set;}
        }
        public class SortInfo
        {
            public string SortBy {get; set;}
            public string SortOrder {get; set;}
        }

        public class Body
        {
            public PaginationInfo pInfo {get; set;}
            public SortInfo sInfo {get; set;}

            public Body()
            {
                pInfo = new PaginationInfo();
                sInfo = new SortInfo();
            }
        }
        private readonly TechOnlineShopContext _context;
    

        public ProductsController(TechOnlineShopContext context)
        {
            _context = context;
        }

        [HttpPatch("")]

        public async Task<ActionResult<Product>> GetProducts(Body body)
        {
            var products  = await _context.Products.ToListAsync();;
            

            if (products == null || products.Count == 0)
            {
                return NotFound();
            }

            switch(body.sInfo.SortOrder)
            {
                case "desc":
                    switch(body.sInfo.SortBy)
                    {
                        case "ProductName":
                        products = products.OrderByDescending( p => p.ProductName).ToList();
                        break;
                        case "Category":
                        products = products.OrderByDescending( p => p.Category).ToList();
                        break;
                        case "ProductAttributes":
                        products = products.OrderByDescending( p => p.ProductAttributes.LastOrDefault()).ToList();
                        break;
                        case "Manufacturers":
                        products = products.OrderByDescending( p => p.Manufacturers.LastOrDefault()).ToList();
                        break;
                        case "Price":
                        products = products.OrderByDescending( p => p.Price).ToList();
                        break;
                        default:
                        products = products.OrderByDescending(p => p.ProductId).ToList();
                        break;
                    }
                break;
                case "asc":
                    switch(body.sInfo.SortBy)
                    {
                        case "ProductName":
                        products = products.OrderBy( p => p.ProductName).ToList();
                        break;
                        case "Category":
                        products = products.OrderBy( p => p.Category).ToList();
                        break;
                        case "ProductAttributes":
                        products = products.OrderBy( p => p.ProductAttributes.FirstOrDefault()).ToList();
                        break;
                        case "Manufacturers":
                        products = products.OrderBy( p => p.Manufacturers.FirstOrDefault()).ToList();
                        break;
                        case "Price":
                        products = products.OrderBy( p => p.Price).ToList();
                        break;
                        default:
                        products = products.OrderBy(p => p.ProductId).ToList();
                        break;
                    }
                break;
            }

            var offset = (body.pInfo.page - 1) * body.pInfo.pageSize;
            var currentPageData = products.Skip(offset).Take(body.pInfo.pageSize).ToList();

            var totalItems = products.Count;
            var totalPages = (int)Math.Ceiling((double)totalItems / body.pInfo.pageSize);

            var response = new
            {
                Data = currentPageData,
                Page = body.pInfo.page,
                PageSize = body.pInfo.pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

            return Ok(response);

        }
    }
}