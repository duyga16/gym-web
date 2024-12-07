using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class Healthinfo
{
    public int HealthId { get; set; }

    public int? UserId { get; set; }

    public decimal? Height { get; set; }

    public decimal? Weight { get; set; }

    public decimal? Bmi { get; set; }
    public int? Age { get; set; }
    public string? Gender { get; set; }
    public decimal? BodyFatPercentage { get; set; }

    public string? Link { get; set; }

    public string? Meta { get; set; }

    public int? Hide { get; set; }

    public int Order { get; set; }

    public DateTime? Datebegin { get; set; }

    public virtual User? User { get; set; }
}
