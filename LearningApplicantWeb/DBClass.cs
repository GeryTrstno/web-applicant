using LearningApplicantWeb.Models.EF;
using Microsoft.EntityFrameworkCore;

namespace LearningApplicantWeb
{
    public class DBClass
    {
        public static string _ConnString = "";

        public static ModelContext GetContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ModelContext>();
            optionsBuilder.UseMySql(_ConnString, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.4.3-mysql"));
            return new ModelContext(optionsBuilder.Options);
        }
    }
}
