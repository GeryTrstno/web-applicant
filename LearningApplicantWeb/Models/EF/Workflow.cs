using System;
using System.Collections.Generic;

namespace LearningApplicantWeb.Models.EF;

public partial class Workflow
{
    public int WorkflowId { get; set; }

    public string? WorkflowSlug { get; set; }

    public string? WorkflowDescription { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
