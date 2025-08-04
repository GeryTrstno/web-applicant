using Microsoft.AspNetCore.Mvc;

namespace LearningApplicantWeb.Controllers
{
    public class ApplicantController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
