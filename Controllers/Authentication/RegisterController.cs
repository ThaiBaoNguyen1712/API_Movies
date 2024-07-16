using Microsoft.AspNetCore.Mvc;

namespace API_Movies.Controllers.Authentication
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
