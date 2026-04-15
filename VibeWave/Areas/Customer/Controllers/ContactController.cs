using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using VibeWave.DataAccess.Repository.IRepository;
using VibeWave.Models;

namespace VibeWave.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ContactController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ContactController(IUnitOfWork unitOfWork)
        {
        _unitOfWork=unitOfWork;
        }
        public IActionResult Index()
        {
            return View(new Contact());
        }
        [HttpPost]
        public IActionResult Index(Contact obj)
        {
            if (obj.FirstName==obj.LastName)
            {
                ModelState.AddModelError("FirstName", "First Name cannot be the same as Last Name");
            }
            if (ModelState.IsValid)
            {
                var message = new ContactMessage
                {
                    FirstName = obj.FirstName,
                    LastName = obj.LastName,
                    Email = obj.Email,
                    Country = obj.Country,
                    Subject = obj.Subject,
                    Message = obj.Message,
                };
                _unitOfWork.ContactMessage.Add(message);
                _unitOfWork.Save();
                TempData["success"] = "Thank you for contacting us! We will get back to you soon.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}
