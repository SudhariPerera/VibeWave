using Microsoft.AspNetCore.Mvc;

namespace VibeWave.Controllers
{
    public class Contact : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
