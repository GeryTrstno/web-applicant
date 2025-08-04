namespace LearningApplicantWeb.Models
{
    public class HomeVM
    {
        public class Index
        {
            public List<EF.Role> Roles { get; set; } = new List<EF.Role>();

            public Index()
            {
                var context = DBClass.GetContext();
                Roles = context.Roles.ToList();

            }
        }
    }
}
