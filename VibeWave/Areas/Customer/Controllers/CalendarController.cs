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

            var concerts = _unitOfWork.Concert.GetAll(includeProperties: "Category").ToList();

            concerts = concerts
                .Where(c => c.DisplayDate.Month == currentMonth && c.DisplayDate.Year == currentYear)
                .ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                concerts = concerts
                    .Where(c => c.ConcertName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                             || c.ActorName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (categoryId.HasValue)
            {
                concerts = concerts.Where(c => c.CategoryId == categoryId.Value).ToList();
            }

            var futureConcerts = concerts.Where(c => c.DisplayDate >= today).OrderBy(c => c.DisplayDate).ToList();
            var pastConcerts = concerts.Where(c => c.DisplayDate < today).OrderBy(c => c.DisplayDate).ToList();

            var calendarVM = new CalendarVM
            {
                SearchString = searchString,
                CategoryId = categoryId,
                CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
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