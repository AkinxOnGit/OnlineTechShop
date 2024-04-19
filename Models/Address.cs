using System;
using System.Collections.Generic;

namespace OnlineTechShop.Models;

public partial class Address
{
    public int AddressId { get; set; }

    public string Street { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;

    public virtual ICollection<OrderTable> Orders { get; set; } = new List<OrderTable>();
}
