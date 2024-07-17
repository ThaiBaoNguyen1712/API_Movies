using System;
using System.Collections.Generic;

namespace API_Movies.Models;

public partial class User
{
    public decimal Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? EmailVerifiedAt { get; set; }

    public int? IsVerified { get; set; }

    public Guid? VerificationCode { get; set; }

    public string Password { get; set; } = null!;

    public string? RememberToken { get; set; }

    public string? Role { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
