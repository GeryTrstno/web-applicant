using System;
using System.Collections.Generic;

namespace LearningApplicantWeb.Models.EF;

public partial class Role
{
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<Workflow> Workflows { get; set; } = new List<Workflow>();
}
