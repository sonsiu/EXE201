namespace Group2_SE1814_NET.Models;

public partial class Order {
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? VoucherId { get; set; }

    public Double TotalAmountBefore { get; set; }

    public decimal? DiscountAmount { get; set; }

    public Double TotalAmountAfter { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PaymentStatus { get; set; }

    public DateTime? EndDate { get; set; }
    public decimal? ShippingFee { get; set; }
    public string? OrderStatus { get; set; }
    public string? Note { get; set; }
    public string? OrderCode { get; set; }
    public string? ProvinceName { get; set; }
    public string? DistrictName { get; set; }
    public string? WardName { get; set; }

    public string? ShopId { get; set; }
    public int? ServiceTypeId { get; set; }



    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual User? User { get; set; }

    public virtual Voucher? Voucher { get; set; }
}
