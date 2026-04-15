using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using VibeWave.DataAccess.Repository.IRepository;
using VibeWave.Models;

namespace VibeWave.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ContactController : Controller
    {
        //private readonly IUnitOfWork _unitOfWork;
        //public ContactController(IUnitOfWork unitOfWork)
        //{
        //_unitOfWork=unitOfWork;
        //}
        public IActionResult Index()
        {
            return View(new Contact());
        }
        [HttpPost]
        public IActionResult Index(Contact obj)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Thank you for contacting us. We will get back to you soon.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}
