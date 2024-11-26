using CSRMGMT.Models;
using Microsoft.AspNetCore.Mvc;
using CSRMGMT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CSRMGMT.Models.MyViewComponentModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CSRMGMT.Component
{
    public class UserInfoViewComponent : ViewComponent
    {
        private readonly AppdbContext _dbcontext1;
        private readonly UserManager<AppUser> _userManager;

        public UserInfoViewComponent(AppdbContext context, UserManager<AppUser> userManager)
        {
            _dbcontext1 = context;
            _userManager = userManager;
        }
        public    async Task<IViewComponentResult> InvokeAsync()
        {
            string UserName = "", Email = "", FullName = "";
            var user =   await _dbcontext1.AppUser.FirstOrDefaultAsync(x => x.UserName == HttpContext.User.Identity.Name);
            if (user != null)
            {
                UserName = user.UserName;
                Email = user.Email;
                FullName = user.Name;

            }
            var data = new MyViewComponentModel
            {
                UserName = UserName,
                Email = Email,
                FullName = FullName
            };

            // Return the view with the model
            return View("_UserInfo.cshtml", data);
        }
    }
}
