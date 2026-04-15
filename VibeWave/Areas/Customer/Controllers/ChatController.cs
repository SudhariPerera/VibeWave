using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using VibeWave.DataAccess.Repository.IRepository;
using VibeWave.Models;

namespace VibeWave.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ChatController : Controller
    {
        //private readonly IUnitOfWork _unitOfWork;
        //public ChatController(IUnitOfWork unitOfWork)
        //{
        //    _unitOfWork = unitOfWork;
        //}
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(ChatInput obj)
        {
           if(string.IsNullWhiteSpace(obj.UserMessage))
            {
                ModelState.AddModelError("UserMessage");
            }
            if (!ModelState.IsValid)
            {
                var message = new ContactMessage
                {
                    return View(obj);
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
