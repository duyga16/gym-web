using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? UserId { get; set; }
    public int? NewsId { get; set; }

    public int? PtId { get; set; }
    public string? Details { get; set; }
    public string? Title { get; set; }
    public int? Rating { get; set; }

    public string? Link { get; set; }

    public string? Meta { get; set; }

    public int? Hide { get; set; }

    public int Order { get; set; }

    public DateTime? Datebegin { get; set; }

    public virtual User? User { get; set; }
    public virtual Pt? PT { get; set; }
    public virtual News? News { get; set; }


}
