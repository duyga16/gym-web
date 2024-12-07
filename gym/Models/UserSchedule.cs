using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class UserSchedule
{
    public int UserScheduleId { get; set; }
    public int UserId { get; set; }

    public int ScheduleId { get; set; }

    public string? Link { get; set; }

    public string? Meta { get; set; }

    public int? Hide { get; set; }

    public int Order { get; set; }

    public DateTime? Datebegin { get; set; }

    public virtual Schdule Schdule { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
