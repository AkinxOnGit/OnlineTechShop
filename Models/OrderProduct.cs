using System;
using System.Collections.Generic;

namespace OnlineTechShop.Models;

public partial class OrderProduct
{
    public int OrderProductId { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal? PurchasePrice { get; set; }

    public virtual OrderTable? Order { get; set; }

    public virtual Product? Product { get; set; }
}
