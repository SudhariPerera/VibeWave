using Microsoft.AspNetCore.Mvc;

namespace VibeWave.Areas.Customer.Controllers
{
    public class Contact : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
