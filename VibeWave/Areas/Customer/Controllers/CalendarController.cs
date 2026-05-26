using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VibeWave.DataAccess.Repository.IRepository;
using VibeWave.Models.ViewModels;

namespace VibeWave.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CalendarController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int? month, int? year, string searchString, int? categoryId)
        {
            int currentMonth = month ?? DateTime.Now.Month;
            int currentYear = year ?? DateTime.Now.Year;

            var today = DateOnly.FromDateTime(DateTime.Now);

            var concertsQuery = _unitOfWork.Concert.GetAll(includeProperties: "Category");

            // SEARCH (global)
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.Trim().ToLower();

                concertsQuery = concertsQuery.Where(c =>
                    c.ConcertName.ToLower().Contains(searchString) ||
                    c.ActorName.ToLower().Contains(searchString));
            }

            // CATEGORY (global)
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                concertsQuery = concertsQuery.Where(c =>
                    c.CategoryId == categoryId.Value);
            }

            // MONTH FILTER (for calendar view)
            var concerts = concertsQuery
                .Where(c => c.DisplayDate.Month == currentMonth &&
                            c.DisplayDate.Year == currentYear)
                .ToList();

            // SORT
            var futureConcerts = concerts
                .Where(c => c.DisplayDate >= today)
                .OrderBy(c => c.DisplayDate);

            var pastConcerts = concerts
                .Where(c => c.DisplayDate < today)
                .OrderByDescending(c => c.DisplayDate);

            var calendarVM = new CalendarVM
            {
                SearchString = searchString,
                CategoryId = categoryId,

                CategoryList = _unitOfWork.Category.GetAll()
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.CategoryId.ToString()
                    }).ToList(),

                Concerts = futureConcerts.Concat(pastConcerts).ToList()
            };

            ViewBag.Month = currentMonth;
            ViewBag.Year = currentYear;

            return View(calendarVM);
        }
    }
}