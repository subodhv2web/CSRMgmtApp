using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using CSRMGMT.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using CSRMGMT.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSRMGMT.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly AppdbContext _context;
        private string Name = "Seastar";
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppdbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._context = context;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult login()
        {
            var items = new List<SelectListItem>
    {
        new SelectListItem { Value = "1", Text = "Option 1" },
        new SelectListItem { Value = "2", Text = "Option 2" },
        new SelectListItem { Value = "3", Text = "Option 3" }
    };
            ViewBag.Items = items;  
            ViewBag.Title = Name + " | Login";
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Title = Name + " | Login";
                var user = await userManager.FindByEmailAsync(model.userid);
                //if (user != null && user.UserName == "admin")
                //{
                //var appUser = await _context.AppUser.FirstOrDefaultAsync(x => x.UserName == model.userid);
                var result = await signInManager.PasswordSignInAsync(model.userid, model.Password, true, false);
                //var result = await _context.AppUser.FirstOrDefaultAsync(u => u.UserId == model.userid
                //&& u.Password == model.Password && u.Status == true);
                string redirectUrl = "/Account/Login";
                if (result.Succeeded)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    var singleRole = roles.FirstOrDefault(); // Get the first role, if exists
                    if (singleRole.ToLower()== "admin")
                    {
                        redirectUrl = "/Admin/Home/Index";
                    }
                    else if(singleRole.ToLower()== "client")
                    {
                        redirectUrl = "/CClient/Home/Index";
                    }
                    else
                    {
                        redirectUrl = "/Agency/Home/Index";
                    }
                    //return RedirectToAction("Admin","index", "home");
                    return RedirectPermanent(redirectUrl);
                }
                else
                {
                    TempData["ErrorMessage"] = "You have entered an invalid username or password";
                    ModelState.AddModelError(string.Empty, "You have entered an invalid username or password");
                    return View(model);
                }
                // }
                //else
                //{
                //    TempData["ErrorMessage"] = "You have entered an invalid username or password";
                //    ModelState.AddModelError(string.Empty, "You have entered an invalid username or password");
                //    return View(model);
                //}
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Redirect(Url.Action("login", "Account"));
        }
    }
}
