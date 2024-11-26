using CSRMGMT.Areas.CClient.ViewModels;
using CSRMGMT.Models;
using CSRMGMT.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CSRMGMT.Areas.Agency.Controllers
{
    [Area("Agency")]
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
            // var projectList = await _context.CsrProject.OrderByDescending(x => x.Id).ToListAsync();
            var projectList = (from c in _context.CsrProject
                               join d in _context.ProjectAllocation on c.Id equals d.ProjectId
                               join e in _context.ProjectAgency on d.AgencyId equals e.Id
                               join lookup in _context.LookupMaster
                               on c.ProjectCategoryId equals lookup.Id into lookupGroup
                               from lookup in lookupGroup.DefaultIfEmpty()  // Left Join
                               where e.Email==User.Identity.Name
                               select new
                               {
                                   c.ProjectName,
                                   c.FilePath,
                                   c.ContactPhone,
                                   c.Location,
                                   c.StartDate,
                                   c.Budget,
                                   c.ContactEmail,
                                   c.ContactPerson,
                                   c.Description,
                                   c.EndDate,
                                   c.Status,
                                   c.Id,
                                   e.Name,
                                   CategoryName=lookup.Name
                               })
                 .ToList();

            return View(projectList);
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

        public async Task<IActionResult> DeleteMilestone(int id)
        {
            var miStTodel = await _context.Milestone.FirstOrDefaultAsync(x => x.Id == id);
            if (miStTodel != null)
            {
                _context.Milestone.Remove(miStTodel);
                await _context.SaveChangesAsync(true);
            }
            TempData["Message"] = AppHelper.DisplayToast("Milestone has been deleted successfully.", "success", "top-right");
            return RedirectPermanent("~/Agency/Project/Milestone/?pid=" + miStTodel.CsrProjectId.ToString());
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

        [HttpGet]
        public async Task<IActionResult> Milestone(MilestoneViewModel model, int pid, int mid)
        {
            model.Milestone = new Milestone();
            model.MilestoneList = _context.Milestone.Where(x => x.CsrProjectId == pid).ToList();
            if (mid > 0)
            {
                model.Milestone = _context.Milestone.FirstOrDefault(x => x.Id == mid);
                if (model.Milestone == null)
                {
                    TempData["Message"] = AppHelper.DisplayToast("Error occured. Try again later.", "error", "top-right");
                }
            }
            ViewBag.CsrProjectId = pid;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Milestone(MilestoneViewModel mvw, int pid)
        {
            string fileName = string.Empty;
            if (mvw.Milestone.StatusFileUpload != null && mvw.Milestone.StatusFileUpload.Length > 0)
            {
                string originalFileName = Path.GetFileNameWithoutExtension(mvw.Milestone.StatusFileUpload.FileName); // Without extension
                string fileExtension = Path.GetExtension(mvw.Milestone.StatusFileUpload.FileName); // Get the file extension

                string truncatedFileName = originalFileName.Length > 5 ? originalFileName.Substring(0, 5) : originalFileName;

                // Generate a unique file name by combining the truncated original file name with a GUID
                string uniqueFileName = truncatedFileName + "_" + Guid.NewGuid().ToString("N") + fileExtension;

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/project", uniqueFileName);
                // Ensure the directory exists
                var directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await mvw.Milestone.StatusFileUpload.CopyToAsync(stream);
                    fileName = uniqueFileName;
                }
            }
            if (!string.IsNullOrEmpty(fileName))
            {
                //mvw.Milestone.StatusFilePath = fileName;
            }
            else
            {
                fileName = mvw.Milestone.StatusFilePath;
            }
            mvw.Milestone.StatusFilePath = fileName;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (mvw.Milestone.Id == 0)
                {
                    _context.Milestone.Add(mvw.Milestone);
                    message = "Milestone has been created successfully.";
                }
                else
                {

                    _context.Update(mvw.Milestone);
                    message = "Milestone has been updated successfully.";
                }
                _context.SaveChanges();
            }
            else
            {
                // Capture errors from the ModelState
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                // Join the error messages into a single string
                message = "Failed to save Milestone. Errors: " + string.Join(", ", errors);
                TempData["Error"] = message;
            }
            // In case of an error, return the current form with validation messages
            var milestones = _context.Milestone.Where(x => x.CsrProjectId == pid).ToList();
            TempData["Message"] = AppHelper.DisplayToast(message, "success", "top-right");
            return RedirectToAction("Milestone", "Project", new { pid = mvw.Milestone.CsrProjectId });
        }

        // GET: ClientController/Edit/5
        public async Task<IActionResult> Details(int id, int AgencyId)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var csrProject =  await (from project in _context.CsrProject join lookup in _context.LookupMaster on project.ProjectCategoryId equals lookup.Id where project.Id==id
                               select new
                               {
                                   project.ProjectName,
                                   project.FilePath,
                                   project.ContactPhone,
                                   project.Location,
                                   project.StartDate,
                                   project.Budget,
                                   project.ContactEmail,
                                   project.ContactPerson,
                                   project.Description,
                                   project.EndDate,
                                   project.Status,
                                   project.Id,
                                   CategoryName = lookup.Name
                               }).SingleAsync();
            if (csrProject == null)
            {
                return NotFound();
            }

            var projAllocation = _context.ProjectAllocation.Include(x => x.Agency).FirstOrDefault(x => x.ProjectId == id);
            if (projAllocation != null)
            {
                ViewBag.Message = "Allocated to agency: " + projAllocation.Agency.Name;
            }

            // Fetch the list of agencies
            var agencyList = await _context.ProjectAgency
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                }).ToListAsync();

            // Pass the data to the view via ViewBag or ViewModel
            ViewBag.Agencies = agencyList;
            return View(csrProject);
        }
    }
}

