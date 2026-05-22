using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VibeWave.Data;
using VibeWave.DataAccess.Repository;
using VibeWave.DataAccess.Repository.IRepository;
using VibeWave.Models;
using VibeWave.Models.ViewModels;

namespace VibeWave.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ConcertController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ConcertController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Concert> objConcertList = _unitOfWork.Concert.GetAll(includeProperties: "Category").ToList();
            return View(objConcertList);
        }

        // GET: Create
        public IActionResult Upsert(int? id)
        {
            ConcertVM concertVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.CategoryId.ToString()
                }),
                Concert = new Concert()
            };
            if (id == null || id == 0)
            {
                return View(concertVM);
            }
            else
            {
                concertVM.Concert = _unitOfWork.Concert.Get(u => u.Id == id);
                return View(concertVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ConcertVM concertVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\concert");

                    if (!string.IsNullOrEmpty(concertVM.Concert.ConcertImageUrl))
                    {
                        //delete the old image by getting the path of that image
                        var oldImagePath = Path.Combine(wwwRootPath, concertVM.Concert.ConcertImageUrl.Trim('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    concertVM.Concert.ConcertImageUrl = @"\images\concert\" + fileName;
                }
                if (concertVM.Concert.Id == 0)
                {
                    _unitOfWork.Concert.Add(concertVM.Concert);
                }
                else
                {
                    _unitOfWork.Concert.Update(concertVM.Concert);
                }
                _unitOfWork.Save();
                TempData["success"] = "Concert Created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                concertVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.CategoryId.ToString()
                });
                return View(concertVM);
            }
        }

        //calender Method
        public IActionResult Calendar(int? month, int? year)
        {
            int currentMonth = month ?? DateTime.Now.Month;
            int currentYear = year ?? DateTime.Now.Year;

            var concerts = _unitOfWork.Concert.GetAll().ToList();

            // Filter concerts for selected month/year
            concerts = concerts
                .Where(c =>
                    c.DisplayDate.Month == currentMonth &&
                    c.DisplayDate.Year == currentYear)
                .ToList();

            ViewBag.Month = currentMonth;
            ViewBag.Year = currentYear;

            return View(concerts);
        }

        public IActionResult Details(int id)
        {
            var concert = _unitOfWork.Concert
                .Get(u => u.Id == id, includeProperties: "Category");

            if (concert == null)
            {
                return NotFound();
            }

            return View(concert);
        }

        #region API calls
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Concert> objConcertList = _unitOfWork.Concert.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objConcertList });
        }

        //delete
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var concertToBeDeleted = _unitOfWork.Concert.Get(u => u.Id == id);
            if (concertToBeDeleted == null)
            {
                return Json(new { success = false, Message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, concertToBeDeleted.ConcertImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Concert.Remove(concertToBeDeleted);

            _unitOfWork.Save();

            return Json(new { success = true, Message = "Delete Successful" });

        }
        #endregion
    }
}