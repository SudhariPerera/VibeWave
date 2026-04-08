using Microsoft.AspNetCore.Mvc;
using VibeWave.Data;
using VibeWave.Models;

namespace VibeWave.Controllers
{
    public class ConcertController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ConcertController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Concert> objConcertList = _db.Concert.ToList();
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
                _db.Concert.Add(obj);
                _db.SaveChanges();
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
            Concert? concertFromDb = _db.Concert.Find(id);
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
                _db.Concert.Update(obj);
                _db.SaveChanges();
                TempData["success"] = " Concert Details Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        //EDIT BUTTON
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Concert? concertFromDb = _db.Concert.Find(id);
            if (concertFromDb == null)
            {
                return NotFound();
            }
            return View(concertFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Concert? obj=_db.Concert.Find(id); 
            if (obj == null) 
            {
                return NotFound();
            }
            _db.Concert.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = " Concert Details Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
