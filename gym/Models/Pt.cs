using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class Pt
{
    public int PtId { get; set; }

    public int? ClassId { get; set; }
    public string NamePt { get; set; } = null!;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Img { get; set; }

    public string? Height { get; set; }

    public string? Weight { get; set; }

    public int? Age { get; set; }

    public string? Gender { get; set; }

    public string? Skills { get; set; }

    public string? Specialty { get; set; }

    public int? ExperienceYears { get; set; }

    public int? Rating { get; set; }

    public string? Descriptions { get; set; }

    public string? Slogan { get; set; }

    public string? Link { get; set; }

    public string? Meta { get; set; }

    public int? Hide { get; set; }

    public int Order { get; set; }

    public DateTime? Datebegin { get; set; }

    public virtual Class? Class { get; set; }
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Schdule> Schdules { get; set; } = new List<Schdule>();

}
