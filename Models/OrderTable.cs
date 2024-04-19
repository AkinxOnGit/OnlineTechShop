using System;
using System.Collections.Generic;

namespace OnlineTechShop.Models;

public partial class OrderTable
{
    public int OrderId { get; set; }

    public int? UserId { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual UserTable? User { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}
