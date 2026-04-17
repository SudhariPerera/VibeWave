using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VibeWave.DataAccess.Repository;
using VibeWave.DataAccess.Repository.IRepository;
using VibeWave.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
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
            List<Concert> objConcertList = _unitOfWork.Concert.GetAll().ToList();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                string keyword = searchString.ToLower();//can search in normal and capital words.
                List<Concert> filteredList = new List<Concert>();//must have
                foreach (var concert in objConcertList)//to see onhttps://blog.csdn.net/William_cl/article/details/156279181
                {
                    if (concert.ConcertName.ToLower().Contains(keyword) || (concert.ActorName != null && concert.ActorName.ToLower().Contains(keyword)))
                    {
                        filteredList.Add(concert);
                    }
                }
                objConcertList = filteredList;
            }
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                List<Concert> filteredList = new List<Concert>();
                foreach (var concert in objConcertList)
                {
                    if (concert.CategoryId == categoryId.Value)
                    {
                        filteredList.Add(concert);
                    }
                }
                objConcertList = filteredList;
            }
            //like ProductController in Bulkyweb
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.CategoryId.ToString()
            });
            ViewBag.CategoryList = CategoryList;
            objConcertList = objConcertList.OrderBy(c => c.DisplayDate).ToList();
            var viewModel = new HomeIndexViewModel
            {
                SearchString = searchString,
                CategoryId = categoryId,
                Concerts = objConcertList
            };
            return View(viewModel);
        }


        public IActionResult Calender()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }

}
