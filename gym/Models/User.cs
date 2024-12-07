using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class User
{
    public int UserId { get; set; }

    public int? AccountId { get; set; }
    public int? RankId { get; set; }

    public int? MenuId { get; set; }

    public string UserName { get; set; } = null!;

    public string? FullName { get; set; }

    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }

    public string? Img { get; set; }
    public decimal? Price { get; set; }
    public decimal? RankPrice { get; set; }
    public int? CountClass { get; set; }
    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? Addess { get; set; }
    public string? Roles { get; set; }

    public string? Link { get; set; }

    public string? Meta { get; set; }

    public int? Hide { get; set; }

    public int Order { get; set; }

    public DateTime? Datebegin { get; set; }

    public virtual Account? Account { get; set; }
    public virtual Rankuser? Rankuser { get; set; }

    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();


    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Gallery> Galleries { get; set; } = new List<Gallery>();

    public virtual ICollection<Healthinfo> Healthinfos { get; set; } = new List<Healthinfo>();

    public virtual Menu? Menu { get; set; }

    public virtual ICollection<UserClass> UserClasses { get; set; } = new List<UserClass>();
    public virtual ICollection<UserSchedule> UserSchedules { get; set; } = new List<UserSchedule>();

}
