namespace Group2_SE1814_NET.Models;

public partial class Comment {
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    public string? CommentContent { get; set; }

    public DateTime? CommentDate { get; set; }

    public int? RepplyCommentId { get; set; }

    public virtual ICollection<Comment> InverseRepplyComment { get; set; } = new List<Comment>();

    public virtual Product? Product { get; set; }

    public virtual Comment? RepplyComment { get; set; }

    public virtual User? User { get; set; }
}
