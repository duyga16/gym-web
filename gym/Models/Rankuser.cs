using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class Rankuser
{
    public int RankId { get; set; }
    public string? MembershipType { get; set; }
    public string? RankType { get; set; }
    public string? Details { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public decimal? Price { get; set; }

    public string? Link { get; set; }

    public string? Meta { get; set; }

    public int? Hide { get; set; }

    public int Order { get; set; }

    public DateTime? Datebegin { get; set; }
    public virtual ICollection<User> Users { get; set; } = new List<User>();


}
