using System;
using System.Collections.Generic;

namespace LearningApplicantWeb.Models.EF;

public partial class JobPosition
{
    public int PositionId { get; set; }

    public string PositionName { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public string? UpdatedBy { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Applicant> Applicants { get; set; } = new List<Applicant>();
}
