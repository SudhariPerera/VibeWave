using Microsoft.AspNetCore.Mvc;
using VibeWave.DataAccess.Repository.IRepository;
using VibeWave.Models;

namespace VibeWave.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Booking/Create/5
        public IActionResult Create(int id)
        {
            Concert? concert = _unitOfWork.Concert.Get(u => u.Id == id);
            if (concert == null)
            {
                return NotFound();
            }
            return View(concert);
        }

        // POST: Booking/Create
        [HttpPost]
        public IActionResult Create(int ConcertId, string CustomerName, string Email, int NumberOfTickets)
        {
            // Check if we received data
            if (ConcertId == 0 || string.IsNullOrEmpty(CustomerName))
            {
                TempData["error"] = "Please fill all fields";
                Concert? concert = _unitOfWork.Concert.Get(u => u.Id == ConcertId);
                return View(concert);
            }

            // Create new booking
            Booking booking = new Booking
            {
                ConcertId = ConcertId,
                CustomerName = CustomerName,
                Email = Email,
                NumberOfTickets = NumberOfTickets,
                BookingDate = DateTime.Now
            };

            _unitOfWork.Booking.Add(booking);
            _unitOfWork.Save();

            TempData["success"] = "Booking Created Successfully!";
            return RedirectToAction("Index", "Home");
        }
    }
}