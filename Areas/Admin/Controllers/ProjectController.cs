using CSRMGMT.Areas.Admin.ViewModels;
using CSRMGMT.Models;
using CSRMGMT.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CSRMGMT.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly ILookupRepository _repository;
        private readonly AppdbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public ProjectController(AppdbContext context, ILookupRepository repository, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _repository = repository;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        // GET: ProjectController
        public async Task<ActionResult> Index()
        {
            //var projectList = await _context.CsrProject.OrderByDescending(x => x.Id).ToListAsync();
            var projectList = await (
            from project in _context.CsrProject
            join lookup in _context.LookupMaster
            on project.ProjectCategoryId equals lookup.Id into lookupGroup
            from lookup in lookupGroup.DefaultIfEmpty()  // Left Join
            orderby project.Id descending
            select new
            {
                project.Id,
                project.ProjectName,
                project.FilePath,
                project.ContactPhone,
                project.Status,
                project.StartDate,
                project.EndDate,
                project.Budget,
                project.ContactEmail,
                project.ContactPerson,
                project.Description,
                project.Location,
                CategoryName = lookup.Name
            }
            ).ToListAsync();
            return View(projectList);
        }

        // GET: ClientController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClientController/Create
        public async Task<IActionResult> Create()
        {
            var clientTypeList = await _repository.GetProjectCategory();
            var selectList = new SelectList(clientTypeList.ToList(), "ID", "Name");
            ViewData["ProjectCategoryList"] = selectList;
            return View();
        }

        // POST: ClientController/Create
        [HttpPost]

        public async Task<IActionResult> Create(CsrProject project)
        {
            string fileName = String.Empty;
            if (project.FileUpload != null && project.FileUpload.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/project", project.FileUpload.FileName);

                // Ensure the directory exists
                //Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await project.FileUpload.CopyToAsync(stream);
                    fileName = project.FileUpload.FileName;
                }
            }
            // try
            {
                CsrProject csrProject = project;
                csrProject.FilePath = "uploads/project/" + fileName;
                await _context.CsrProject.AddAsync(csrProject);
                await _context.SaveChangesAsync();

                TempData["Message"] = AppHelper.DisplayToast("Project has been created successfully.", "success", "top-full");
                //return RedirectToAction(nameof(Index));
            }
            //catch
            //{

            //}
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var csrProject = await _context.CsrProject.FindAsync(id);
            if (csrProject == null)
            {
                return NotFound();
            }
            var clientTypeList = await _repository.GetProjectCategory();
            var selectList = new SelectList(clientTypeList.ToList(), "ID", "Name");
            ViewData["ProjectCategoryList"] = selectList;
            return View(csrProject);
        }

        // POST: ClientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CsrProject csrProject)
        {
            if (id != csrProject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = string.Empty;
                    if (csrProject.FileUpload != null && csrProject.FileUpload.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/project", csrProject.FileUpload.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await csrProject.FileUpload.CopyToAsync(stream);
                            fileName = csrProject.FileUpload.FileName;
                        }
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            csrProject.FilePath = "uploads/project/" + fileName;
                        }
                    }
                    _context.Update(csrProject);
                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    throw;
                }
                TempData["Message"] = AppHelper.DisplayToast("Project has been updated successfully.", "success", "top-full");
                return RedirectToAction(nameof(Index)); // Redirect to a list or details view
            }

            return View(csrProject);
        }
        private bool CorporateClientExists(int id)
        {
            return _context.Client.Any(e => e.Id == id);
        }

        // GET: ClientController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var clientToDel = await _context.Client.FirstOrDefaultAsync(x => x.Id == id);
            if (clientToDel != null)
            {
                _context.Client.Remove(clientToDel);
                await _context.SaveChangesAsync(true);

                var user = await _userManager.FindByNameAsync(clientToDel.SPOCEmail);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: ClientController/Delete/5
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

        public IActionResult Milestone(int id)
        {
            var modelCtr = new List<Milestone>
            {
                new Milestone() // Default milestone with empty values
            };
            var model = _context.Milestone.Where(x => x.CsrProjectId == id).ToList();
            if (model == null || model.Count == 0)
            {
                model.Add(new Milestone());
            }
            ViewBag.CsrProjectId = id;
            return View(model);
        }

        // POST: Milestones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Milestone(List<Milestone> milestones)
        {
            if (ModelState.IsValid)
            {
                _context.UpdateRange(milestones);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(milestones);
        }
    }
}

