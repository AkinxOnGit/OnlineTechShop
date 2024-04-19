using System;
using System.Collections.Generic;

namespace OnlineTechShop.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int? CategoryId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? StockCount { get; set; }

    public string? ProductImage { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual ICollection<ProductAttribute> ProductAttributes { get; set; } = new List<ProductAttribute>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; } = new List<ShoppingCartProduct>();

    public virtual ICollection<Manufacturer> Manufacturers { get; set; } = new List<Manufacturer>();
}
