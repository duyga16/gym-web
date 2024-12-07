using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class Gallery
{
    public int GalleryId { get; set; }

    public int? UserId { get; set; }

    public string? LinkImg { get; set; }

    public int? IsWide { get; set; }
    public string? Link { get; set; }

    public string? Meta { get; set; }

    public int? Hide { get; set; }

    public int Order { get; set; }

    public DateTime? Datebegin { get; set; }

    public virtual User? User { get; set; }
}
