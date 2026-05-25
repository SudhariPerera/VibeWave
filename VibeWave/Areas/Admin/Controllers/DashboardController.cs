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
            var viewModel = new DashboardViewModel
            {
                TotalConcerts = _unitOfWork.Concert.GetAll().Count(),
                TotalCategories = _unitOfWork.Category.GetAll().Count(),
                TotalBookings = _unitOfWork.Booking.GetAll().Count(),
                TotalUsers = _userManager.Users.Count(),
                TotalMessages = _unitOfWork.ContactMessage.GetAll().Count()
            };

            return View(viewModel);
        }
    }
}
