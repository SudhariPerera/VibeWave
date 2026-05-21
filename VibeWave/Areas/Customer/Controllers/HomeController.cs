using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VibeWave.DataAccess.Repository.IRepository;
using VibeWave.Models;
using System.Linq;

namespace VibeWave.Areas.Customer.Controllers
{
    [Area("Customer")]
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

            // 👇 PUT YOUR SEARCH CODE HERE
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.ToLower();

                concerts = concerts.Where(c =>
                    c.ConcertName.ToLower().Contains(searchString) ||
                    (c.ActorName != null && c.ActorName.ToLower().Contains(searchString))
                );
            }

            // CATEGORY FILTER (below search)
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                concerts = concerts.Where(c => c.CategoryId == categoryId.Value);
            }

            var viewModel = new HomeIndexViewModel
            {
                SearchString = searchString,
                CategoryId = categoryId,
                Concerts = concerts.OrderBy(c => c.DisplayDate).ToList(),
                CategoryList = _unitOfWork.Category.GetAll()
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.CategoryId.ToString()
                    })
            };

            return View(viewModel);
        }

        public IActionResult Calender()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}