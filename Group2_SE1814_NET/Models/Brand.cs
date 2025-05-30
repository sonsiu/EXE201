using System;
using System.Collections.Generic;

namespace Group2_SE1814_NET.Models;

public partial class Brand
{
    public int? Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
