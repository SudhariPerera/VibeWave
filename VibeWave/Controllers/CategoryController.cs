using Microsoft.AspNetCore.Mvc;
using VibeWave.Data;
using VibeWave.DataAccess.Repository.IRepository;
using VibeWave.Models;

namespace VibeWave.Controllers
{
    //[Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create( )
        {
             return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            //if (obj.CategoryName == obj.Category.ToString())
            //{
            //    ModelState.AddModelError("Category Name", "The Category Category cannot exactly match with the Category Name");
            //}
            if (ModelState.IsValid)
            {
                obj.CategoryId = 0;//AI advise me to change
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = " Category Details Created Successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        //EDIT BUTTON
        public IActionResult Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.CategoryId == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = " Category Details Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        //Delete Button
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.CategoryId == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.CategoryId == id);
            if (obj == null) 
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = " Category Details Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
