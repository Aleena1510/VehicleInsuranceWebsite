using InsurancePortal.Models;
using Microsoft.AspNetCore.Mvc;
namespace InsurancePortal.Controllers
{
	public class ReportController : Controller
	{
		public IActionResult Confirmation()
		{
			var customer = HttpContext.Session.GetObject<Customer>("CustomerData");
			var vehicle = HttpContext.Session.GetObject<Vehicle>("VehicleData");
			var estimate = HttpContext.Session.GetObject<Estimate>("EstimateData");
			var billing = HttpContext.Session.GetObject<CustomerBilling>("BillingData");

			ViewBag.CustomerData = customer;
			ViewBag.VehicleData = vehicle;
			ViewBag.EstimateData = estimate;
			ViewBag.BillingData = billing;

			return View();
		}
	}
}



