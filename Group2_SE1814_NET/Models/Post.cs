using System;
using System.Collections.Generic;

namespace Group2_SE1814_NET.Models;

public partial class Post
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? CategoryId { get; set; }

    public string? Title { get; set; }

    public string? ShortContent { get; set; }

    public string? FullContent { get; set; }

    public string? Thumbnail { get; set; }

    public DateTime PublishDate { get; set; }

    public virtual Category? Category { get; set; }

    public virtual User? User { get; set; }
}
