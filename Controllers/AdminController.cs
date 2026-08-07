using InsurancePortal.Data;
using InsurancePortal.Models;
using InsurancePortal.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace InsurancePortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly InsurancePortalContext _context;

        public AdminController(InsurancePortalContext context)
        {
            _context = context;
        }

        // ✅ DASHBOARD (unchanged)
        public async Task<IActionResult> Index()
        {
            var stats = new DashboardViewModel
            {
                TotalCustomers = await _context.Customers.CountAsync(),
                TotalVehicles = await _context.Vehicles.CountAsync(),
                TotalPolicies = await _context.CustomerPolicies.CountAsync(),
                TotalBillings = await _context.CustomerBillings.SumAsync(b => b.Amount),
                TotalClaims = await _context.ClaimDetails.CountAsync(),
                TotalEstimates = await _context.Estimates.CountAsync(),
                RecentCustomers = await _context.Customers.OrderByDescending(c => c.CustomerId).Take(5).ToListAsync(),
                RecentPolicies = await _context.CustomerPolicies.OrderByDescending(p => p.PolicyNumber).Take(5).ToListAsync()
            };
            return View(stats);
        }

        // ✅ NEW UNIFIED CUSTOMER DETAILS VIEW
        public async Task<IActionResult> CustomerDetails(int? id)
        {
            if (id == null) return RedirectToAction("Customers");

            // ✅ SEPARATE QUERIES - No navigation properties needed
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
            if (customer == null) return NotFound();

            var customerViewModel = new CustomerDetailsViewModel
            {
                Customer = customer,
                Vehicles = await _context.Vehicles.Where(v => v.CustomerId == id).ToListAsync(),
                Policies = await _context.CustomerPolicies.Where(p => p.CustomerId == id).ToListAsync(),
                Billings = await _context.CustomerBillings.Where(b => b.CustomerId == id).OrderByDescending(b => b.BillNo).ToListAsync(),
                Estimates = await _context.Estimates.Where(e => e.CustomerId == id).OrderByDescending(e => e.EstimateNumber).ToListAsync()
            };

            return View(customerViewModel);
        }


        // ✅ CUSTOMERS LIST - Redirect to unified view
        public async Task<IActionResult> Customers()
        {
            var customers = await _context.Customers
                .OrderByDescending(c => c.CustomerId)
                .ToListAsync();
            return View(customers);
        }



        // ✅ REPORTS (Unchanged - Perfect!)
        public async Task<IActionResult> MonthlySales()
        {
            var sales = await _context.CustomerBillings
                .GroupBy(b => b.Date.Substring(0, 7))
                .Select(g => new ReportViewModel
                {
                    Title = g.Key,
                    Value = g.Sum(b => b.Amount),
                    Count = g.Count()
                })
                .OrderByDescending(s => s.Title)
                .ToListAsync();
            ViewBag.ReportTitle = "📊 Monthly Sales Report";
            return View("Reports", sales);
        }

        public async Task<IActionResult> VehicleAnalysis()
        {
            var analysis = await _context.CustomerPolicies
                .GroupBy(p => p.VehicleName)
                .Select(g => new ReportViewModel
                {
                    Title = g.Key,
                    Value = g.Sum(p => p.VehicleRate),
                    Count = g.Count()
                })
                .OrderByDescending(a => a.Count)
                .ToListAsync();
            ViewBag.ReportTitle = "🚗 Vehicle Wise Analysis";
            return View("Reports", analysis);
        }

        public async Task<IActionResult> ClaimsReport()
        {
            var claims = await _context.ClaimDetails.OrderByDescending(c => c.ClaimNumber).ToListAsync();
            ViewBag.ReportTitle = "⚠️ Claims Report";
            return View("ClaimsReport", claims);
        }

        public async Task<IActionResult> DueRenewals()
        {
            var allPolicies = await _context.CustomerPolicies.ToListAsync();
            var dueRenewals = allPolicies
                .Where(p => {
                    if (DateTime.TryParse(p.PolicyDate, out DateTime policyDate))
                    {
                        return policyDate.AddMonths(p.PolicyDuration).Date <= DateTime.Now.AddDays(30).Date;
                    }
                    return false;
                })
                .OrderBy(p => DateTime.Parse(p.PolicyDate).AddMonths(p.PolicyDuration))
                .ToList();
            return View("DueRenewals", dueRenewals);
        }

        public async Task<IActionResult> LapsedPolicies()
        {
            var allPolicies = await _context.CustomerPolicies.ToListAsync();
            var lapsed = allPolicies
                .Where(p => {
                    if (DateTime.TryParse(p.PolicyDate, out DateTime policyDate))
                    {
                        return policyDate.AddMonths(p.PolicyDuration).Date < DateTime.Now.Date;
                    }
                    return false;
                })
                .OrderByDescending(p => p.PolicyNumber)
                .ToList();
            return View("LapsedPolicies", lapsed);
        }

        // ✅ DELETE CUSTOMER (with cascade)
        [HttpPost]
        [Route("Admin/DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return Json(new { success = false });

            var vehicles = _context.Vehicles.Where(v => v.CustomerId == id);
            var policies = _context.CustomerPolicies.Where(p => p.CustomerId == id);
            var billings = _context.CustomerBillings.Where(b => b.CustomerId == id);
            var estimates = _context.Estimates.Where(e => e.CustomerId == id);

            _context.Vehicles.RemoveRange(vehicles);
            _context.CustomerPolicies.RemoveRange(policies);
            _context.CustomerBillings.RemoveRange(billings);
            _context.Estimates.RemoveRange(estimates);

            _context.Customers.Remove(customer);

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
        // LIST CONTACTS
        public IActionResult Contacts()
        {
            var contacts = _context.Contacts.ToList();
            return View(contacts);
        }

        // DELETE CONTACT
        public IActionResult DeleteContact(int id)
        {
            var contact = _context.Contacts.FirstOrDefault(x => x.Id == id);

            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                _context.SaveChanges();
            }

            return RedirectToAction("Contacts");
        }
        public async Task<IActionResult> ApprovePolicy(int id)
        {
            var policy = await _context.CustomerPolicies.FirstOrDefaultAsync(p => p.PolicyId == id);
            if (policy == null) return NotFound();

            policy.Status = "Approved";
            await _context.SaveChangesAsync();

            return RedirectToAction("CustomerDetails", new { id = policy.CustomerId });
        }

        public async Task<IActionResult> RejectPolicy(int id)
        {
            var policy = await _context.CustomerPolicies.FirstOrDefaultAsync(p => p.PolicyId == id);
            if (policy == null) return NotFound();

            policy.Status = "Rejected";
            await _context.SaveChangesAsync();

            return RedirectToAction("CustomerDetails", new { id = policy.CustomerId });
        }
    }



}




