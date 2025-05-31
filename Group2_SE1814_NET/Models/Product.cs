namespace Group2_SE1814_NET.Models;

public partial class Product {
    public int Id { get; set; }

    public int? BrandId { get; set; }

    public int? CategoryId { get; set; }

    public string? Name { get; set; }

    public string? ImageUrl { get; set; }

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public string? ShortDescription { get; set; }

    public string? Description { get; set; }

    public bool Status { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
