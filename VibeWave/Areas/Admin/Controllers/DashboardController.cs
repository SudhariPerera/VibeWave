using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VibeWave.DataAccess.Repository.IRepository;
using VibeWave.Models;
using VibeWave.Data;

namespace VibeWave.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var bookings = _unitOfWork.Booking.GetAll();

            var viewModel = new DashboardViewModel
            {
                TotalConcerts = _unitOfWork.Concert.GetAll().Count(),
                TotalCategories = _unitOfWork.Category.GetAll().Count(),
                TotalBookings = bookings.Count(),
                TotalUsers = _userManager.Users.Count(),
                TotalMessages = _unitOfWork.ContactMessage.GetAll().Count(),

                TotalRevenue = bookings.Where(b => b.IsPaid).Sum(b => b.TotalPrice),
                PaidBookings = bookings.Count(b => b.IsPaid),
                PendingBookings = bookings.Count(b => !b.IsPaid),
                 RefundedBookings = bookings.Count(b => b.PaymentStatus == "Refunded")
            };

            return View(viewModel);
        }
    }
}
