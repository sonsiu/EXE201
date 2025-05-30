using System;
using System.Collections.Generic;

namespace Group2_SE1814_NET.Models;

public partial class Image
{
    public int Id { get; set; }

    public int? FeedbackId { get; set; }

    public string? ImageUrl { get; set; }

    public virtual Feedback? Feedback { get; set; }
}
