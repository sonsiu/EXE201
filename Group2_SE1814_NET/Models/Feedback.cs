namespace Group2_SE1814_NET.Models;

public partial class Feedback {
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? OrderId { get; set; }

    public int? Rating { get; set; }

    public string? FeedbackContent { get; set; }

    public DateTime? FeedbackDate { get; set; }

    public int? ReplyFeedbackId { get; set; }

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<Feedback> InverseReplyFeedback { get; set; } = new List<Feedback>();

    public virtual Order? Order { get; set; }

    public virtual Feedback? ReplyFeedback { get; set; }

    public virtual User? User { get; set; }
}
