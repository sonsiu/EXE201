using System;
using System.Collections.Generic;

namespace Group2_SE1814_NET.Models;

public partial class Role
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
