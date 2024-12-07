using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class News
{
    public int NewsId { get; set; }

    public string NewsName { get; set; } = null!;

    public string? Img { get; set; }

    public string Descriptions { get; set; } = null!;

    public string Script { get; set; } = null!;

    public string? Detail { get; set; }

    public string? Title { get; set; }

    public int? IsLike { get; set; }

    public string? Slogan { get; set; }

    public string? Link { get; set; }

    public string? subMeta { get; set; }

    public string? Meta { get; set; }

    public int? Hide { get; set; }

    public int Order { get; set; }

    public DateTime? Datebegin { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
