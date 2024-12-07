using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public int? UserId { get; set; }

    public string NameCourse { get; set; } = null!;

    public string Tiltle { get; set; } = null!;

    public string? Price { get; set; }

    public string Details { get; set; } = null!;

    public string? Link { get; set; }

    public string? Meta { get; set; }

    public int? Hide { get; set; }

    public int Order { get; set; }

    public DateTime? Datebegin { get; set; }

    public virtual User? User { get; set; }
}
