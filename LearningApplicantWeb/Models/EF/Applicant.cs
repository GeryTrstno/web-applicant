using System;
using System.Collections.Generic;

namespace LearningApplicantWeb.Models.EF;

public partial class Applicant
{
    public int ApplicantId { get; set; }

    public int PositionId { get; set; }

    public string RegisterCode { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Nik { get; set; } = null!;

    public string BirthPlace { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public int Gender { get; set; }

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Education { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public string? UpdatedBy { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ApplicantStatus? ApplicantStatus { get; set; }

    public virtual JobPosition Position { get; set; } = null!;
}
