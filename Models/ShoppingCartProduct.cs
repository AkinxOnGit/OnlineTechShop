using System;
using System.Collections.Generic;

namespace OnlineTechShop.Models;

public partial class ShoppingCartProduct
{
    public int ShoppingCartProductsId { get; set; }

    public int? ShoppingCartId { get; set; }

    public int? ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Product? Product { get; set; }

    public virtual ShoppingCart? ShoppingCart { get; set; }
}
