﻿namespace Group2_SE1814_NET.Models;

public partial class Cart {
    public int Id { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual User? User { get; set; }
}
