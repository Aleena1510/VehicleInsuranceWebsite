using InsurancePortal.Data;
using Microsoft.AspNetCore.Mvc;
using InsurancePortal.Models;

namespace InsurancePortal.Controllers
{
    public class ContactController : Controller
    {
        private readonly InsurancePortalContext _context;

        public ContactController(InsurancePortalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Contact model)
        {
            if (ModelState.IsValid)
            {
                _context.Contacts.Add(model);
                await _context.SaveChangesAsync();

                // Set flag to show popup on next request
                TempData["ShowSuccessPopup"] = true;

                // Redirect to GET action to avoid resubmission on refresh
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
