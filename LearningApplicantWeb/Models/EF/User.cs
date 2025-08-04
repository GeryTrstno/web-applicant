using System;
using System.Collections.Generic;

namespace LearningApplicantWeb.Models.EF;

public partial class User
{
    public string Username { get; set; } = null!;

    public string? Password { get; set; }

    public int RoleId { get; set; }

    public virtual Role Role { get; set; } = null!;
}
