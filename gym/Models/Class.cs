using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class Class
{
    public int ClassId { get; set; }

    public string NameClass { get; set; } = null!;

    public string? Img { get; set; }

    public decimal? Price { get; set; }
    public int? CountUser { get; set; }
    public string Descriptions { get; set; } = null!;

    public string? Detail { get; set; }

    public string? Title { get; set; }

    public string? Link { get; set; }

    public string? Meta { get; set; }

    public int? Hide { get; set; }

    public int Order { get; set; }

    public DateTime? Datebegin { get; set; }

    public virtual ICollection<Pt> Pts { get; set; } = new List<Pt>();

    public virtual ICollection<Schdule> Schdules { get; set; } = new List<Schdule>();

    public virtual ICollection<UserClass> UserClasses { get; set; } = new List<UserClass>();
}
