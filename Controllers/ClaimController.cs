using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using InsurancePortal.Data;
using InsurancePortal.Models;

namespace InsurancePortal.Controllers
{
    public class ClaimController : Controller
    {
        private readonly InsurancePortalContext _context;

        public ClaimController(InsurancePortalContext context)
        {
            _context = context;
        }

        // GET: Claim/Index
        [HttpGet]
        public IActionResult Index()
        {
            return View(new ClaimDetail());
        }

        // POST: Claim/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ClaimDetail model)
        {
            // Remove any FK validation if present
            ModelState.Remove("CustomerId");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.ClaimDetails.Add(model);
                    _context.SaveChanges();

                    TempData["Success"] = $"Claim #{model.ClaimNumber} registered successfully!";
                    return RedirectToAction("Success");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error: {ex.Message}";
                    Console.WriteLine($"Claim Error: {ex.Message}");
                }
            }
            else
            {
                TempData["Error"] = "Please fill all required fields correctly.";
            }

            return View(model);
        }

        // GET: Claim/Success
        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }

       
    }
}