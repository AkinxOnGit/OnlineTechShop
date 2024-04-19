using System;
using System.Collections.Generic;

namespace OnlineTechShop.Models;

public partial class ShoppingCart
{
    public int ShoppingCartId { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreationDate { get; set; }

    public virtual ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; } = new List<ShoppingCartProduct>();

    public virtual UserTable? User { get; set; }
}
