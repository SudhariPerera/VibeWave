using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using VibeWave.DataAccess.Repository.IRepository;
using VibeWave.Models;
using VibeWave.Models.ViewModels;

namespace VibeWave.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string searchString, int? categoryId)
        {
            var concerts = _unitOfWork.Concert
           .GetAll(includeProperties: "Category")
           .AsQueryable();

            // search and filter
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.ToLower();
                concerts = concerts.Where(c =>
                    c.ConcertName.ToLower().Contains(searchString) ||
                    (c.ActorName != null && c.ActorName.ToLower().Contains(searchString))
                );
            }

            if (categoryId.HasValue && categoryId.Value > 0)
                concerts = concerts.Where(c => c.CategoryId == categoryId.Value);

            var today = DateOnly.FromDateTime(DateTime.Today);

            var concertList = concerts
                .OrderBy(c => c.DisplayDate)
                .Select(c => new HomeConcertVM
                {
                    Id = c.Id,
                    ConcertName = c.ConcertName,
                    ActorName = c.ActorName,
                    ConcertLocation = c.ConcertLocation,
                    DisplayDate = c.DisplayDate,                      
                    DisplayTime = c.DisplayTime.ToString("hh\\:mm tt"), 
                    TicketPrice = c.TicketPrice,
                    CategoryName = c.Category.Name,
                    ConcertImageUrl = c.ConcertImageUrl,
                    IsBookable = c.DisplayDate >= today             
                })
                .ToList();

            var viewModel = new HomeIndexViewModel
            {
                SearchString = searchString,
                CategoryId = categoryId,
                Concerts = concertList, 
                CategoryList = _unitOfWork.Category.GetAll()
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.CategoryId.ToString()
                    })
            };

            return View(viewModel);
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Calendar()
        {
            return View();
        }
    }
}