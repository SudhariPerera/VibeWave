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
        //asyns+await 这是一个处理用户注册请求的异步动作方法，接收表单提交的注册数据，执行用户创建逻辑，并根据结果返回视图、重定向或错误信息。
        public async Task<IActionResult> Register(RegisterViewModel obj)
        {
            if(obj.Password!=obj.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");
            }
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = obj.Email,
                    Email = obj.Email,
                    PhoneNumber=obj.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, obj.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent:false);//Cookie（关闭浏览器即退出）
                    TempData["success"] = "Registration successful!";
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(obj);
        }
        //get customer, account, login
        public IActionResult Login(string returnUrl=null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel obj,string returnUrl=null)
        {
            ViewBag.ReturnUrl = returnUrl;
            if(ModelState.IsValid)
            {
                var result=await _signInManager.PasswordSignInAsync(
                    obj.Email,
                    obj.Password,
                    obj.RememberMe,
                    lockoutOnFailure:false
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
    }
}
