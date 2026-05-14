using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using VibeWave.DataAccess.Repository.IRepository;
using VibeWave.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace VibeWave.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AccountController : Controller
    {
        //private readonly IUnitOfWork _unitOfWork;
        //public AccountController(IUnitOfWork unitOfWork)
        //{
        //    _unitOfWork = unitOfWork;
        //}
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AccountController(UserManager<IdentityUser>userManager,SignInManager<IdentityUser>signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //get the customer, account, register
        public IActionResult Register()
        {
            return View();
        }

        //get the customer, account, register

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel obj)
        {
            if (obj.Password != obj.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");
            }

            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = obj.Email,
                    Email = obj.Email,
                    PhoneNumber = string.IsNullOrWhiteSpace(obj.PhoneNumber) ? null : obj.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, obj.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["success"] = "Registration successful!";
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(obj);
        }

        //[HttpPost]
        //public async Task<IActionResult> Register(RegisterViewModel obj)
        //{
        //    // 确认密码一致性
        //    if (obj.Password != obj.ConfirmPassword)
        //    {
        //        ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");
        //    }

        //    // 移除电话号码所有验证错误（因为是选填）
        //    if (ModelState.ContainsKey("PhoneNumber"))
        //    {
        //        ModelState["PhoneNumber"].Errors.Clear();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var user = new IdentityUser
        //        {
        //            UserName = obj.Email,
        //            Email = obj.Email,
        //            PhoneNumber = string.IsNullOrWhiteSpace(obj.PhoneNumber) ? null : obj.PhoneNumber
        //        };

        //        System.Diagnostics.Debug.WriteLine("Calling CreateAsync...");
        //        var result = await _userManager.CreateAsync(user, obj.Password);
        //        System.Diagnostics.Debug.WriteLine($"CreateAsync finished. Succeeded = {result.Succeeded}");

        //        if (!result.Succeeded)
        //        {
        //            System.Diagnostics.Debug.WriteLine("--- Identity Errors ---");
        //            foreach (var err in result.Errors)
        //            {
        //                System.Diagnostics.Debug.WriteLine($"Code: {err.Code}, Description: {err.Description}");
        //            }
        //        }

        //        if (result.Succeeded)
        //        {
        //            System.Diagnostics.Debug.WriteLine("Signing in...");
        //            await _signInManager.SignInAsync(user, isPersistent: false);
        //            TempData["success"] = "Registration successful!";
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            // 添加 Identity 返回的错误（跳过电话号码相关）
        //            foreach (var error in result.Errors)
        //            {
        //                if (error.Code.Contains("PhoneNumber", StringComparison.OrdinalIgnoreCase))
        //                    continue;
        //                ModelState.AddModelError(string.Empty, error.Description);
        //            }
        //        }   
        //    }

        //    return View(obj);
        //}

        //[HttpPost]
        ////asyns+await 这是一个处理用户注册请求的异步动作方法，接收表单提交的注册数据，执行用户创建逻辑，并根据结果返回视图、重定向或错误信息。
        //public async Task<IActionResult> Register(RegisterViewModel obj)
        //{
        //    if (obj.Password != obj.ConfirmPassword)
        //    {
        //        ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");
        //    }

        //    if (ModelState.ContainsKey("PhoneNumber"))
        //    {
        //        ModelState["PhoneNumber"].Errors.Clear();
        //    }

        //    //if (ModelState.ContainsKey("PhoneNumber"))
        //    //{
        //    //ModelState.Remove("PhoneNumber");
        //        //    var phoneErrors = ModelState["PhoneNumber"].Errors
        //        //        .Where(e => e.ErrorMessage != null &&
        //        //               e.ErrorMessage.Contains("required", StringComparison.OrdinalIgnoreCase))
        //        //        .ToList();

        //        //    foreach (var error in phoneErrors)
        //        //    {
        //        //        ModelState["PhoneNumber"].Errors.Remove(error);
        //        //    }
        //        //}


        //        if (ModelState.IsValid)
        //        {
        //            var user = new IdentityUser
        //            {
        //                UserName = obj.Email,
        //                Email = obj.Email,
        //                PhoneNumber = string.IsNullOrWhiteSpace(obj.PhoneNumber) ? null : obj.PhoneNumber
        //            };

        //            var result = await _userManager.CreateAsync(user, obj.Password);

        //            if (result.Succeeded)
        //            {
        //                await _signInManager.SignInAsync(user, isPersistent: false);//Cookie（关闭浏览器即退出）
        //                TempData["success"] = "Registration successful!";
        //                return RedirectToAction("Index", "Home");
        //            }
        //            else
        //            {
        //            // 强制添加一条测试错误，确认摘要能显示
        //            ModelState.AddModelError(string.Empty, "TEST: CreateAsync failed, check output window for details.");

        //            foreach (var error in result.Errors)
        //                {
        //                    if (error.Code.Contains("PhoneNumber", StringComparison.OrdinalIgnoreCase))
        //                        continue;

        //                    ModelState.AddModelError(string.Empty, error.Description);
        //                }
        //            }
        //        }
        //        return View(obj);
        //    }
        //}
        //get customer, account, login
        public IActionResult Login(string returnUrl=null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]//Login 方法
        public async Task<IActionResult> Login(LoginViewModel obj,string returnUrl=null)
        {
            ViewBag.ReturnUrl = returnUrl;
            if(ModelState.IsValid)
            {
                var result=await _signInManager.PasswordSignInAsync(
                    obj.Email,
                    obj.Password,
                    obj.RememberMe,
                    lockoutOnFailure:true
                    );
                if (result.Succeeded)
                {
                    TempData["success"] = "Login successful";
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index","Home");//改变浏览器 URL，使其指向首页。防止刷新时重复执行 POST 操作。
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(obj);
        }
        //post customer, account, login
        [HttpPost]
        public async Task<IActionResult>Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["success"] = "You have been logged out.";
            return RedirectToAction("Index", "Home");
        }
        //get customer, account, login
        public async Task<IActionResult>Index()
        {
            var user=await _userManager.GetUserAsync(User);
            if(user==null)
            {
                return RedirectToAction("Login");
            }
            return View(user);
        }

        public IActionResult ForgotPassword()
        {
            TempData["info"] = "Password reset feature is coming soon.";
            return RedirectToAction("Login");
        }

        // 占位：重发邮件确认
        public IActionResult ResendEmailConfirmation()
        {
            TempData["info"] = "Email confirmation feature is coming soon.";
            return RedirectToAction("Login");
        }
    }
}
