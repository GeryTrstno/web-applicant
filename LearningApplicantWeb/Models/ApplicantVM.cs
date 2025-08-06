using LearningApplicantWeb.Models.EF;
using System.ComponentModel.DataAnnotations;

namespace LearningApplicantWeb.Models
{
    public class ApplicantVM
    {
        public class Index
        {
            public List<Applicant> Applicants { get; set; } = new List<Applicant>();
        }
    }
}
