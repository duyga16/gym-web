using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string? Roles { get; set; }
    public DateTime? ResetTokenExpires { get; set; }
    public string? ResetToken { get; set; }

    public int? AcceptTerms { get; set; } = null!;

    public int? IsEmailConfirmed { get; set; } = null!;

    public string? EmailConfirmationToken { get; set; }
    public string? PhoneNumber { get; set; }
    public int? RoleAdmin { get; set; }
    public string? Link { get; set; }

    public string? Meta { get; set; }

    public int? Hide { get; set; }

    public int Order { get; set; }

    public DateTime? Datebegin { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
