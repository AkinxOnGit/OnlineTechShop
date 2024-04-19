using System;
using System.Collections.Generic;

namespace OnlineTechShop.Models;

public partial class UserTable
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? RegistrationDate { get; set; }

    public virtual ICollection<OrderTable> OrderTables { get; set; } = new List<OrderTable>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
}
