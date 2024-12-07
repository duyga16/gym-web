using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class UserClass
{
    public int UserId { get; set; }

    public int ClassId { get; set; }

    public string? Link { get; set; }

    public string? Meta { get; set; }

    public int? Hide { get; set; }

    public int Order { get; set; }

    public DateTime? Datebegin { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
