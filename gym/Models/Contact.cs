using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class Contact
{
    public int ContactId { get; set; }

    public int? UserId { get; set; }
    public string? PhoneNumber { get; set; }
    public string UserName { get; set; } = null!;
    public string Title { get; set; } = null!;

    public string Details { get; set; } = null!;

    public string? Link { get; set; }

    public string? Meta { get; set; }

    public int? Hide { get; set; }

    public int Order { get; set; }

    public DateTime? Datebegin { get; set; }

    public virtual User? User { get; set; }
}
