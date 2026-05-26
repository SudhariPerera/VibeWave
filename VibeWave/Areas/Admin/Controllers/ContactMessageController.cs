using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VibeWave.DataAccess.Repository.IRepository;
using VibeWave.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using VibeWave.Data;


namespace VibeWave.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContactMessageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContactMessageController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // 列表页：显示所有留言
        public IActionResult Index()
        {
            var messages = _unitOfWork.ContactMessage.GetAll()
                .OrderByDescending(m => m.SubmittedAt)
                .ToList();
            return View(messages);
        }

        // 详情页：查看单条留言并标记为已读
        public IActionResult Details(int id)
        {
            var message = _unitOfWork.ContactMessage.Get(m => m.Id == id);
            if (message == null)
                return NotFound();

            // 如果未读，标记为已读
            if (!message.IsRead)
            {
                message.IsRead = true;
                _unitOfWork.ContactMessage.Update(message);
                _unitOfWork.Save();
            }

            return View(message);
        }

        // 删除留言
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var message = _unitOfWork.ContactMessage.Get(m => m.Id == id);
            if (message == null)
                return NotFound();

            _unitOfWork.ContactMessage.Remove(message);
            _unitOfWork.Save();
            TempData["success"] = "Message deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}