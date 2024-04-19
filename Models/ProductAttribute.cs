using System;
using System.Collections.Generic;

namespace OnlineTechShop.Models;

public partial class ProductAttribute
{
    public int ProductAttributesId { get; set; }

    public int? AttributeId { get; set; }

    public int? ProductId { get; set; }

    public virtual Attribute? Attribute { get; set; }

    public virtual Product? Product { get; set; }
}
