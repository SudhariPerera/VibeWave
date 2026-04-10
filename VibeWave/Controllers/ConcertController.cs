using Microsoft.AspNetCore.Mvc;
using VibeWave.Data;
using VibeWave.DataAccess.Repository.IRepository;
using VibeWave.Models;

namespace VibeWave.Controllers
{
    public class ConcertController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ConcertController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Concert> objConcertList = _unitOfWork.Concert.GetAll().ToList();
            return View(objConcertList);
        }

        public IActionResult Create( )
        {
             return View();
        }

        [HttpPost]
        public IActionResult Create(Concert obj)
        {
            if (obj.ConcertName == obj.ConcertCategory.ToString())
            {
                ModelState.AddModelError("Concert Name", "The Concert Category cannot exactly match with the Concert Name");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Concert.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = " Concert Details Created Successfully";
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
            Concert? concertFromDb = _unitOfWork.Concert.Get(u => u.Id == id);
            if (concertFromDb == null)
            {
                return NotFound();
            }
            return View(concertFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Concert obj)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.Concert.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = " Concert Details Updated Successfully";
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
            Concert? concertFromDb = _unitOfWork.Concert.Get(u => u.Id == id);
            if (concertFromDb == null)
            {
                return NotFound();
            }
            return View(concertFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Concert? obj = _unitOfWork.Concert.Get(u => u.Id == id);
            if (obj == null) 
            {
                return NotFound();
            }
            _unitOfWork.Concert.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = " Concert Details Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
