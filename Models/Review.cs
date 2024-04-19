using System;
using System.Collections.Generic;

namespace OnlineTechShop.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int? ProductId { get; set; }

    public int? UserId { get; set; }

    public int? Stars { get; set; }

    public string? Comment { get; set; }

    public virtual Product? Product { get; set; }

    public virtual UserTable? User { get; set; }
}
