using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CSRMGMT.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace CSRMGMT.Areas.Agency.Controllers
{
    [Area("Agency")]
    [Authorize] 
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppdbContext _context;
        public HomeController(AppdbContext context,ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Myprofile()
        {
            var myProfile = (from client in _context.ProjectAgency.Include(x=>x.State)
                             where client.Email == HttpContext.User.Identity.Name
                             select new
                             {
                                 Client = client,
                             })
                 .FirstOrDefault();
            return View(myProfile);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
