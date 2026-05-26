using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VibeWave.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VibeWave.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // 用户列表
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userList = new List<UserWithRoles>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserWithRoles
                {
                    UserId = user.Id,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles
                });
            }
            return View(userList);
        }

        // 编辑角色（GET）
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();

            var model = new UserManagementViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                CurrentRoles = currentRoles,
                AllRoles = allRoles
            };

            return View(model);
        }

        // 编辑角色（POST）
        [HttpPost]
        public async Task<IActionResult> Edit(UserManagementViewModel model, string[] selectedRoles)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);

            // 移除所有现有角色
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // 添加选中的角色
            if (selectedRoles != null && selectedRoles.Length > 0)
                await _userManager.AddToRolesAsync(user, selectedRoles);

            TempData["success"] = $"Roles updated for {user.Email}.";
            return RedirectToAction("Index");
        }

        // 删除用户
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            // 可选：不允许删除自己
            if (User.Identity.Name == user.Email)
            {
                TempData["error"] = "You cannot delete your own account.";
                return RedirectToAction("Index");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                TempData["success"] = $"User {user.Email} deleted.";
            else
                TempData["error"] = "Failed to delete user.";
            return RedirectToAction("Index");
        }
    }
}