using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using CSRMGMT.Areas.Admin.ViewModels;
using CSRMGMT.Models;
using CSRMGMT.ViewModel;
using System.Drawing.Imaging;
using System.Web;

namespace CSRMGMT.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CmsController : Controller
    {
        private readonly AppdbContext _context;
        private readonly IFileProvider _fileprovider;
        private IConfiguration _configuration;
        string Name = "Seastar";
        public CmsController(AppdbContext context, IFileProvider fileprovider, IConfiguration configration)
        {
            this._context = context;
            this._fileprovider = fileprovider;
            this._configuration = configration;
        }

        // GET: AdminController
        public ActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ManagePages(int id)
        {

            List<LinksInfoViewModel> myList = new List<LinksInfoViewModel>();
            var li = _context.tblLinksInfo.ToList();

            foreach (var itm in li)
            {
                string lnkStClass = "text-success";
                string sLinkStatus = "In Active";
                if (itm.IsLinkActive == true)
                {
                    lnkStClass = "text-success";
                    sLinkStatus = "Active";
                }
                else
                {
                    sLinkStatus = "Inactive";
                    lnkStClass = "text-danger";
                }
                string linkContents=HttpUtility.HtmlDecode(itm.LinkContents);
                string linkType = "";
                var lt = await _context.tblLinkTypes.FirstOrDefaultAsync(x => x.LinkTypeId == itm.sLinkTypeID);
                if (lt!=null)
                {
                    linkType=lt.LinkTypeName;
                }
                myList.Add(new LinksInfoViewModel {LinkLastUpdate=itm.LinkLastUpdate, sLinkID = itm.sLinkID, sLinkStatus = sLinkStatus, sLinkName = itm.sLinkName, sLinkStatusClass = lnkStClass, LinkContents= linkContents, ExURL=itm.ExURL, linkType=linkType});
            }

            return View(myList);
        }
        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        //public ActionResult NewPage()
        //{
        //    return View();
        //}

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Editpage(int? id)
        {
    //        var items = new List<SelectListItem>
    //{
    //    new SelectListItem { Value = "1", Text = "Option 1" },
    //    new SelectListItem { Value = "2", Text = "Option 2" },
    //    new SelectListItem { Value = "3", Text = "Option 3" }
    //};

            //ViewBag.Items = items;
            var linkTypes = (from c in _context.tblLinkTypes
                             select new 
                             {
                                 Text = c.LinkTypeName,
                                 Value = c.LinkTypeId.ToString(),
                             }).ToList();

            var linksList = await (from c in _context.tblLinksInfo
                                   select new 
                                   {
                                       Text = c.sLinkName,
                                       Value = c.sLinkID.ToString(),
                                   }).ToListAsync();

            ViewBag.LtList = linkTypes;
            linksList.Add(new 
            {
                Text = "Select",
                Value = "0",
            });
            ViewBag.LinkList = linksList.OrderBy(x=>x.Value);
            if (id == null)
            {
                ViewBag.Title = Name + " | Add Page Contents";
                ViewBag.IsimgEmpty = true;
                ViewBag.IsNew = true;
                ViewBag.ButtonName = "Save";
            }
            else
            {
                ViewBag.Title = _configuration.GetValue("AppSettings", "ApplicationName") + " | Update Page Contents";
                string filename = string.Empty;
                //string AbsPath = manUtils.GetAppSetting("Application", "AbsPath");
                // var Path = AbsPath + "wwwroot/images/";
                //var Path = AbsPath + "/images/";
                LinksInfo model = new LinksInfo();
                var Info = await _context.tblLinksInfo.FirstOrDefaultAsync(x => x.sLinkID == id);
                //if (Info.ImagePath != null)
                //{
                //    filename = Info.ImagePath;
                //    //     filename = Path + Info.ImagePath;
                //}
                if (Info != null)
                {
                    model.sLinkID = Info.sLinkID;
                    model.sLinkName = Info.sLinkName;
                    model.sLinkTypeID = Info.sLinkTypeID;
                    model.ParentLinkID = Info.ParentLinkID;
                    //    model.ImagePath = manUtils.GetStoryThumbForEditor4Category(filename);
                    //model.ImagePath = filename;
                    model.UseURL = Info.UseURL;
                    model.InNewWindow = Info.InNewWindow;
                    model.UseURL = Info.UseURL;
                    model.ExURL = Info.ExURL;
                    model.LinkContents = System.Net.WebUtility.HtmlDecode(Info.LinkContents);
                    model.IsLinkActive = Info.IsLinkActive;
                    model.WindowTitle = Info.WindowTitle;
                    model.MetaKeywords = Info.MetaKeywords;
                    model.MetaDescription = Info.MetaDescription;
                    ViewBag.IsimgEmpty = false;
                }
                ViewBag.IsNew = false;
                ViewBag.ButtonName = "Update";
                return View(model);
            }


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Editpage(int? Id, LinksInfo model)
        {

            if (ModelState.IsValid)
            {
                string filename = string.Empty;
                if (Id == null)
                {
                    //try
                    //{
                    ViewBag.Title = _configuration.GetValue("AppSettings", "ApplicationName") + "| Add Page Contents";
                    bool useUrl = false;
                    bool newWindow = false;
                    string exUrl = string.Empty;
                    //if (file != null)
                    //{ filename = await GenFuncs.UploadImage(file); }
                    if (model.UseURL == true)
                    {
                        useUrl = true;
                        exUrl = model.ExURL;
                        newWindow = model.InNewWindow;
                    }
                    LinksInfo info = new LinksInfo();

                    info.sLinkName = model.sLinkName;
                    //info.PageIdentifier = "hello";
                    //info.ImagePath = file.FileName;
                    //info.ResizeImagePath = filename;
                    info.UseURL = useUrl;
                    info.ExURL = exUrl;
                    info.InNewWindow = newWindow;
                    info.sLinkTypeID = model.sLinkTypeID;
                    info.ParentLinkID = model.ParentLinkID; 
                    info.LinkContents = System.Net.WebUtility.HtmlEncode(model.LinkContents);
                    info.IsLinkActive = model.IsLinkActive;
                    info.WindowTitle = model.WindowTitle;
                    info.MetaKeywords = model.MetaKeywords;
                    info.MetaDescription = model.MetaDescription;
                    info.LinkCreatedOn = System.DateTime.Now;
                    info.LinkLastUpdate = System.DateTime.Now;
                    info.ParentLinkID = model.sLinkID;
                    _context.Add(info);
                    await _context.SaveChangesAsync();
                    //ToastMessage message = new ToastMessage()
                    //{
                    //    Title = "Created!",
                    //    Message = "Create New CMS Page Successfully at " + DateTime.Now.ToString() + ".",
                    //    Type = "success"
                    //};
                    // TempData["Message"] = Notification.Show(message.Message, message.Title, position: Position.TopRight, type: ToastType.Success, timeOut: 7000);
                    TempData["Message"] = AppHelper.DisplayToast("Page has been created successfully.", "success", "top-full");
                    //}
                    //catch (Exception Ex)
                    //{
                    //    string exceptionMessage = string.Empty;
                    //    if (Ex != null)
                    //    {
                    //        if (Ex.InnerException != null)
                    //            exceptionMessage = Ex.InnerException.Message;
                    //        else
                    //            exceptionMessage = Ex.Message;
                    //        if (!string.IsNullOrEmpty(exceptionMessage))
                    //            exceptionMessage = string.Format("With Error:{0}", HttpUtility.HtmlEncode(exceptionMessage));
                    //        ModelState.AddModelError("", exceptionMessage);
                    //        //ToastMessage message = new ToastMessage()
                    //        //{
                    //        //    Title = "Failed!",
                    //        //    Message = "Create New CMS Page Failed at " + DateTime.Now.ToString() + "." + exceptionMessage,
                    //        //    Type = "error"
                    //        //};
                    //        //TempData["Message"] = Notification.Show(message.Message, message.Title, position: Position.TopRight, type: ToastType.Error, timeOut: 7000);
                    //        //return View();
                    //    }
                    //}
                }
                else
                {
                    try
                    {
                        ViewBag.Title = Name + " | Edit Page Contents";
                        string AbsPath = _configuration.GetValue("AppSettings", "AbsPath");
                        var Path = AbsPath + "wwwroot/images/";
                        var Info = await _context.tblLinksInfo.FirstOrDefaultAsync(x => x.sLinkID == Id);

                        bool newWindow = false;
                        bool useUrl = false;
                        string exUrl = string.Empty;
                        
                        if (model.UseURL == true)
                        {
                            useUrl = true;
                            exUrl = model.ExURL;
                            newWindow = model.InNewWindow;
                        }
                        Info.sLinkName = model.sLinkName;
                        Info.ParentLinkID = model.ParentLinkID;
                        //Info.PageIdentifier = model.PageIdentifier;
                        //if (file != null)
                        //{
                        //    filename = await GenFuncs.UploadImage(file);
                        //    Info.ImagePath = file.FileName;
                        //    Info.ResizeImagePath = filename;
                        Info.sLinkTypeID = model.sLinkTypeID;
                        Info.InNewWindow = newWindow;
                        Info.UseURL = useUrl;
                        Info.ExURL = exUrl;
                        Info.LinkContents = System.Net.WebUtility.HtmlEncode(model.LinkContents);
                        Info.IsLinkActive = model.IsLinkActive;
                        Info.WindowTitle = model.WindowTitle;
                        Info.MetaKeywords = model.MetaKeywords;
                        Info.MetaDescription = model.MetaDescription;
                        Info.LinkLastUpdate = System.DateTime.Now;
                        _context.tblLinksInfo.Update(Info);
                        //ToastMessage message = new ToastMessage()
                        //{
                        //    Title = "Update Successful!",
                        //    Message = "Update New CMS Page Successfully at " + DateTime.Now.ToString() + ".",
                        //    Type = "success"
                        //};
                        //TempData["Message"] = Notification.Show(message.Message, message.Title, position: Position.TopRight, type: ToastType.Success, timeOut: 7000);

                        _context.SaveChanges();
                         TempData["Message"] = AppHelper.DisplayToast("Page has been updated successfully.", "success", "top-full");
                    }
                    catch (Exception Ex)
                    {
                        string exceptionMessage = string.Empty;
                        if (Ex != null)
                        {
                            if (Ex.InnerException != null)
                                exceptionMessage = Ex.InnerException.Message;
                            else
                                exceptionMessage = Ex.Message;
                            if (!string.IsNullOrEmpty(exceptionMessage))
                                exceptionMessage = string.Format("With Error:{0}", HttpUtility.HtmlEncode(exceptionMessage));
                            //ToastMessage message = new ToastMessage()
                            //{
                            //    Title = "Update Failed!",
                            //    Message = "Update CMS Page failed at " + DateTime.Now.ToString() + "." + exceptionMessage,
                            //    Type = "error"
                            //};
                            TempData["Message"] = exceptionMessage;
                            return View();
                        }
                    }
                }
            }
            else
            {
                foreach (var error in ViewData.ModelState.Values)
                {
                    foreach (var error1 in error.Errors)
                    {
                        ModelState.AddModelError("", error1.ErrorMessage);
                    }
                }
            }
            var linkTypes = (from c in _context.tblLinkTypes
                             select new SelectListItem()
                             {
                                 Text = c.LinkTypeName,
                                 Value = c.LinkTypeId.ToString(),
                             }).ToList();

            var linksList = (from c in _context.tblLinksInfo
                             select new SelectListItem()
                             {
                                 Text = c.sLinkName,
                                 Value = c.sLinkID.ToString(),
                             }).ToList();
            ViewBag.ltList = linkTypes;
            linksList.Add(new SelectListItem()
            {
                Text = "Select",
                Value = "0",
            });
            ViewBag.LinkList = linksList;
            return View();
        }
    }
}
