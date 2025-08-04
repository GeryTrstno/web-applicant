using Microsoft.AspNetCore.Mvc;

namespace LearningApplicantWeb.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
