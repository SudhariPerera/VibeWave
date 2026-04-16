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

        //AI
        [HttpGet]
        public IActionResult Index()
        {
            if (TempData["ResponseMessage"] != null)
            {
                ViewBag.Response = TempData["ResponseMessage"].ToString();
            }
            return View(new ChatInput());
        }

        //Made by myself
        [HttpPost]
        public IActionResult Index(ChatInput obj)
        {
            if (string.IsNullOrWhiteSpace(obj.UserMessage))
            {
                ModelState.AddModelError("UserMessage", "Message cannot be empty.");
            }
            if (ModelState.IsValid)
            {
                var botResponse = GenerateResponse(obj.UserMessage);

                TempData["success"] = "Message sent to bot.";
                TempData["ResponseMessage"] = botResponse;
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        private string GenerateResponse(string userMessage)
        {
            string msg = userMessage.ToLower();

            if (msg.Contains("hello"))
            {
                return "Hi there! How can I help you?";
            }
            else if (msg.Contains("help"))
            {
                return "You can ask me about our concerts, tickets, or contact information.";
            }
            else if (msg.Contains("concert"))
            {
                return "We have various concerts available. Check our homepage for the list!";
            }
            else if (msg.Contains("ticket"))
            {
                return "You can book tickets from the concert details page.";
            }
            else if (msg.Contains("contact"))
            {
                return "You can reach us via the Contact Us page.";
            }
            else if (msg.Contains("price"))
            {
                return "Ticket prices vary by concert. Please see individual concert pages.";
            }
            else
            {
                return "I'm not sure how to answer that. Try asking about 'concert', 'ticket', 'help', or 'contact'.";
            }
        }
    }
}
