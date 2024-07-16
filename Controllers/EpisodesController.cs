using Microsoft.AspNetCore.Mvc;

namespace API_Movies.Controllers
{
    public class EpisodesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
