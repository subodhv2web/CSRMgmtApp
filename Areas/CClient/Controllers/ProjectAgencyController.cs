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
    [Area("CClient")]
    [Authorize]
    public class ProjectAgencyController : Controller
    {
        private readonly ILookupRepository _repository;
        private readonly AppdbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public ProjectAgencyController(AppdbContext context, ILookupRepository repository, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _repository = repository;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        // GET: ProjectController
        public async Task<ActionResult> Index()
        {
            var projectAgencyList = await _context.ProjectAgency.Include(x => x.State).OrderByDescending(x => x.Id).ToListAsync();
            return View(projectAgencyList);
        }

        // GET: ClientController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClientController/Create
        public async Task<IActionResult> Create()
        {
            var stateList = _context.State
            .Select(c => new SelectListItem
            {
                Text = c.StateName,
                Value = c.Id.ToString()
            }).ToList();
            // Add a blank option at the beginning
            stateList.Insert(0, new SelectListItem { Text = "Select a state", Value = "" });
            ViewData["StateList"] = new SelectList(stateList, "Value", "Text");
            return View();
        }

        // POST: ClientController/Create
        [HttpPost]
        public async Task<IActionResult> Create(ProjectAgency projectAgency)
        {
            if (ModelState.IsValid)
            {
                

                // Create the Identity user
                var user = new AppUser
                {
                    UserName = projectAgency.Email,
                    Email = projectAgency.Email // Optional: Sync email with Identity
                };
                string password = AppHelper.GeneratePassword(8);
                var getUser = await _userManager.FindByNameAsync(projectAgency.Email);
                if(getUser==null)
                {
                    //Add Project Agency
                    await _context.ProjectAgency.AddAsync(projectAgency);
                    await _context.SaveChangesAsync();
                    //Add User
                    var result = await _userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Agency");
                        var appUser = await _context.AppUser.FirstOrDefaultAsync(x => x.UserName == projectAgency.Email);
                        if (appUser != null)
                        {
                            appUser.Password = password;
                            appUser.Name = projectAgency.Email;
                            await _context.SaveChangesAsync();
                        }
                        TempData["Message"] = AppHelper.DisplayToast("Project Agency has been created successfully.", "success", "top-full");
                    }
                }
                else
                {
                    TempData["Message"] = AppHelper.DisplayToast("Email already exists.", "error", "top-full");
                }
                

                
                //return RedirectToAction(nameof(Index));
            }
            else
            {
                // Get the list of errors from the ModelState
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                // Concatenate error messages and display them in TempData
                TempData["ErrorMessages"] = "Failed to create Project Agency. Please fix the errors below.<br><br>" + string.Join("<br>", errorMessages);
                TempData["Message"] = AppHelper.DisplayToast("Failed to create Project Agency. Please fix the errors below.", "error", "top-full");
            }
            var stateList = _context.State
            .Select(c => new SelectListItem
            {
                Text = c.StateName,
                Value = c.Id.ToString()
            }).ToList();

            // Add a blank option at the beginning
            stateList.Insert(0, new SelectListItem { Text = "Select a state", Value = "" });
            ViewData["StateList"] = new SelectList(stateList, "Value", "Text");
            return View();
        }

        // GET: ClientController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var projectAgency = await _context.ProjectAgency
            .Include(pa => pa.State)
            .SingleOrDefaultAsync(pa => pa.Id == id);
            if (projectAgency == null)
            {
                return NotFound();
            }
            var stateList = _context.State
            .Select(c => new SelectListItem
            {
                Text = c.StateName,
                Value = c.Id.ToString()
            }).ToList();
            // Add a blank option at the beginning
            stateList.Insert(0, new SelectListItem { Text = "Select a state", Value = "" });
            ViewData["StateList"] = new SelectList(stateList, "Value", "Text");
            return View(projectAgency);
        }

        // POST: ClientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectAgency projectAgency)
        {
            if (id != projectAgency.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(projectAgency);
                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    throw;
                }
                TempData["Message"] = AppHelper.DisplayToast("Project Agency has been updated successfully.", "success", "top-full");
                return RedirectToAction(nameof(Index)); // Redirect to a list or details view
            }

            return View(projectAgency);
        }


        // GET: ClientController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var clientToDel = await _context.ProjectAgency.FirstOrDefaultAsync(x => x.Id == id);
            if (clientToDel != null)
            {

                if (clientToDel.IsActive)
                {
                    clientToDel.IsActive= false;
                }
                else
                {
                    clientToDel.IsActive = true;
                }
                await _context.SaveChangesAsync(true);
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

