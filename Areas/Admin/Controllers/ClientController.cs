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
    public class ClientController : Controller
    {
        private readonly ILookupRepository _repository;
        private readonly AppdbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public ClientController(AppdbContext context, ILookupRepository repository, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _repository = repository;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        // GET: ClientController
        public async Task<ActionResult> Index()
        {
            var clientList = await _context.Client.ToListAsync();
            foreach (var item in clientList)
            {
                var luMaster = await _context.LookupMaster.FirstOrDefaultAsync(x => x.Id == item.ClientTypeId && x.MasterName == "ClientType");
                if (luMaster != null)
                {
                    item.ClientType = luMaster.Name;
                }
            }
            return View(clientList);
        }

        // GET: ClientController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClientController/Create
        public async Task<IActionResult> Create()
        {
            var clientTypeList = await _repository.GetClientType();
            //var ct = (from c in clientType
            //                 select new
            //                 {
            //                     Text = c.Name,
            //                     Value = c.ID,
            //                 }).ToList();
            //ViewBag.ClientType = ct;
            var selectList = new SelectList(clientTypeList.ToList(), "ID", "Name");
            ViewData["ClientTypeList"] = selectList;
            return View();
        }

        // POST: ClientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            // try
            {
                Client mClient = client;

                mClient.CreatedDate = DateTime.Now;
                await _context.Client.AddAsync(mClient);
                await _context.SaveChangesAsync();

                // Create the Identity user
                var user = new AppUser
                {
                    UserName = client.SPOCEmail,
                    Email = client.SPOCEmail // Optional: Sync email with Identity
                };
                string password = AppHelper.GeneratePassword(8);
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Client");
                    var appUser = await _context.AppUser.FirstOrDefaultAsync(x => x.UserName == client.SPOCEmail);
                    if (appUser != null)
                    {
                        appUser.Password = password;
                        appUser.Name = client.SPOCName;
                        await _context.SaveChangesAsync();
                    }
                }
                TempData["Message"] = AppHelper.DisplayToast("Client has been created successfully.", "success", "top-full");
                //return RedirectToAction(nameof(Index));
            }
            //catch
            //{

            //}
            return View();
        }

        // GET: ClientController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }

            var corporateClient = await _context.Client.FindAsync(id);
            if (corporateClient == null)
            {
                return NotFound();
            }
            var clientTypeList = await _repository.GetClientType();
            var selectList = new SelectList(clientTypeList.ToList(), "ID", "Name");
            ViewData["ClientTypeList"] = selectList;
            return View(corporateClient);
        }

        // POST: ClientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyName,Address,City,State,PostalCode,PhoneNumber,Email,CreatedDate,ModifiedDate,Notes,Industry,Website,IsActive,SPOCName,SPOCPhoneNumber,SPOCEmail,SPOCPosition,ClientTypeId")] Client corporateClient)
        {
            if (id != corporateClient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Set the ModifiedDate to the current date and time
                    corporateClient.ModifiedDate = DateTime.UtcNow;
                    _context.Update(corporateClient);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CorporateClientExists(corporateClient.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Message"] = AppHelper.DisplayToast("Client has been updated successfully.", "success", "top-full");
                return RedirectToAction(nameof(Index)); // Redirect to a list or details view
            }

            return View(corporateClient);
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
    }
}
