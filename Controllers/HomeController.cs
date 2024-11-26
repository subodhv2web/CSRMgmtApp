using AspNetCore.Reporting;
using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;
using CCA.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSRMGMT.Dataset;
using CSRMGMT.Models;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using RestSharp;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Specialized;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using Azure;

namespace CSRMGMT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnv;
        private readonly AppdbContext _context;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnv, AppdbContext context)
        {
            _logger = logger;
            _webHostEnv = webHostEnv;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public string SplitPay()
        {
            ViewData["Message"] = "Success";
            return "success";
        }
        public IActionResult Page(string title, int? id)
        {
            var pageContent = _context.tblLinksInfo.FirstOrDefault(x => x.sLinkID == id);

            if (pageContent != null)
            {
                ViewBag.PageTitle = pageContent.sLinkName;
                ViewBag.PageContent = HttpUtility.HtmlDecode(pageContent.LinkContents);
            }
            return View();
        }
        public IActionResult DownloadReport()
        {
            var ds1 = (from c in _context.AppUser select new { Id = c.Id, Name = c.Name, Email = c.Email }).ToList();
            //RenderType rt = new RenderType();
            //rt.Equals("pdf");
            string format = "PDF";
            string extension = "pdf";
            string mimetype = "application/pdf";
            string reportPath = _webHostEnv.WebRootPath + "/Reports/Report1.rdlc";
            var localreport = new LocalReport(reportPath);
            localreport.AddDataSource("DataSet1", ds1);
            var res = localreport.Execute(RenderType.Pdf, 1, null, mimetype);
            return File(res.MainStream, mimetype);
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
