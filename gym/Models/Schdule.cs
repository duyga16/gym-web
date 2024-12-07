using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class Schdule
{
    public int ScheduleId { get; set; }

    public int ClassId { get; set; }

    public int? PtId { get; set; }

    public string DayOfWeeks { get; set; } = null!;

    public string TimeSlot { get; set; } = null!;

    public string? NameClass { get; set; }

    public string? NamePt { get; set; }

    public DateOnly? DateCreate { get; set; }

    public string? Link { get; set; }

    public string? Meta { get; set; }

    public int? Hide { get; set; }

    public int Order { get; set; }

    public DateTime? Datebegin { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual Pt? Pt { get; set; }

    public virtual ICollection<UserSchedule> UserSchedules { get; set; } = new List<UserSchedule>();

}
