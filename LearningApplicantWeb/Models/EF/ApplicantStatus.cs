using System;
using System.Collections.Generic;

namespace LearningApplicantWeb.Models.EF;

public partial class ApplicantStatus
{
    public int ApplicantId { get; set; }

    public bool IsApproved { get; set; }

    public string Description { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public string? UpdatedBy { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Applicant Applicant { get; set; } = null!;
}
