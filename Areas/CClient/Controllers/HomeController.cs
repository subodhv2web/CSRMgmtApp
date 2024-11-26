using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CSRMGMT.Models;
using System.Diagnostics;

namespace CSRMGMT.Areas.CClient.Controllers
{
    [Area("CClient")]
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
            var myProfile = (from client in _context.Client
                             join lookup in _context.LookupMaster
                             on client.ClientTypeId equals lookup.Id
                             where client.Email == HttpContext.User.Identity.Name
                             && lookup.MasterName == "ClientType"
                             select new
                             {
                                 Client = client,
                                 ClientType = lookup.Name
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
