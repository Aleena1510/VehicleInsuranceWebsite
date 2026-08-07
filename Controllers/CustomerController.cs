using InsurancePortal.Data;
using InsurancePortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InsurancePortal.Controllers
{
	[Authorize(Roles = "User")]
	public class CustomerController : Controller
	{
		private readonly InsurancePortalContext _context;

		public CustomerController(InsurancePortalContext context)
		{
			_context = context;
		}

		// ================= HELPER =================
		private int GetCustomerIdOrThrow()
		{
			var cid = HttpContext.Session.GetInt32("CustomerId");
			if (!cid.HasValue || cid == 0)
				throw new Exception("Session expired. Please start again.");
			return cid.Value;
		}

		// ================= STEP 1: CUSTOMER =================
		[HttpGet]
		public IActionResult Index()
		{
			var cid = HttpContext.Session.GetInt32("CustomerId");
			Customer customer = null;

			if (cid.HasValue)
				customer = _context.Customers.Find(cid.Value);

			if (customer == null)
				customer = GetCustomerFromSession();

			return View(customer ?? new Customer());
		}

		[HttpPost]
		public IActionResult Index(Customer customer)
		{
			if (!ModelState.IsValid)
				return View(customer);

			if (customer.CustomerId > 0)
			{
				var existing = _context.Customers.Find(customer.CustomerId);
				if (existing != null)
					_context.Entry(existing).CurrentValues.SetValues(customer);
			}
			else
			{
				_context.Customers.Add(customer);
			}

			_context.SaveChanges();

			HttpContext.Session.SetInt32("CustomerId", customer.CustomerId);
			StoreCustomerInSession(customer);

			return RedirectToAction("Vehicle");
		}

		// ================= STEP 2: VEHICLE =================
		[HttpGet]
		public IActionResult Vehicle()
		{
			var cid = GetCustomerIdOrThrow();

			ViewBag.CustomerId = cid;
			ViewBag.CustomerData = GetCustomerFromSession();

			var vid = HttpContext.Session.GetInt32("VehicleId");
			var vehicle = vid.HasValue ? _context.Vehicles.Find(vid.Value) : null;

			return View(vehicle ?? new Vehicle { CustomerId = cid });
		}

		[HttpPost]
		public IActionResult Vehicle(Vehicle vehicle)
		{
			var cid = GetCustomerIdOrThrow();
			vehicle.CustomerId = cid;

			if (!ModelState.IsValid)
			{
				ViewBag.CustomerData = GetCustomerFromSession();
				ViewBag.CustomerId = cid;
				return View(vehicle);
			}

			if (vehicle.VehicleId > 0)
			{
				var existing = _context.Vehicles.Find(vehicle.VehicleId);
				if (existing != null)
					_context.Entry(existing).CurrentValues.SetValues(vehicle);
			}
			else
			{
				_context.Vehicles.Add(vehicle);
			}

			_context.SaveChanges();

			HttpContext.Session.SetInt32("VehicleId", vehicle.VehicleId);
			StoreVehicleInSession(vehicle);

			return RedirectToAction("Estimate");
		}

		// ================= STEP 3: ESTIMATE =================
		[HttpGet]
		public IActionResult Estimate()
		{
			var cid = GetCustomerIdOrThrow();

			ViewBag.CustomerData = GetCustomerFromSession();
			ViewBag.VehicleData = GetVehicleFromSession();
			ViewBag.EstimateNumber = $"EST-{DateTime.Now:yyMMdd}-{cid:D4}";

			var est = _context.Estimates.FirstOrDefault(e => e.CustomerId == cid);

			return View(est ?? new Estimate { CustomerId = cid });
		}

		[HttpPost]
		public IActionResult Estimate(Estimate est)
		{
			var cid = GetCustomerIdOrThrow();
			est.CustomerId = cid;

			if (!ModelState.IsValid)
			{
				ViewBag.CustomerData = GetCustomerFromSession();
				ViewBag.VehicleData = GetVehicleFromSession();
				return View(est);
			}

			if (est.EstimateId > 0)
			{
				var existing = _context.Estimates.Find(est.EstimateId);
				if (existing != null)
					_context.Entry(existing).CurrentValues.SetValues(est);
			}
			else
			{
				_context.Estimates.Add(est);
			}

			_context.SaveChanges();
			StoreEstimateInSession(est);

			return RedirectToAction("Policy");
		}

		// ================= STEP 4: POLICY =================
		[HttpGet]
		public IActionResult Policy()
		{
			var cid = GetCustomerIdOrThrow();

			ViewBag.CustomerData = GetCustomerFromSession();
			ViewBag.VehicleData = GetVehicleFromSession();
			ViewBag.EstimateData = GetEstimateFromSession();
			ViewBag.PolicyNumber = $"POL-{DateTime.Now:yyyy}-{cid:D6}";

			var pol = _context.CustomerPolicies.FirstOrDefault(p => p.CustomerId == cid);

			return View(pol ?? new CustomerPolicy { CustomerId = cid });
		}

		[HttpPost]
		public IActionResult Policy(CustomerPolicy pol)
		{
			var cid = GetCustomerIdOrThrow();
			pol.CustomerId = cid;

			if (!ModelState.IsValid)
			{
				ViewBag.CustomerData = GetCustomerFromSession();
				ViewBag.VehicleData = GetVehicleFromSession();
				ViewBag.EstimateData = GetEstimateFromSession();
				return View(pol);
			}

			// FILE UPLOAD
			if (pol.CustomerAddProveFile != null)
			{
				string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
				if (!Directory.Exists(folder))
					Directory.CreateDirectory(folder);

				string fileName = Guid.NewGuid() + Path.GetExtension(pol.CustomerAddProveFile.FileName);
				string path = Path.Combine(folder, fileName);

				using (var stream = new FileStream(path, FileMode.Create))
				{
					pol.CustomerAddProveFile.CopyTo(stream);
				}

				pol.CustomerAddProvePath = "/uploads/" + fileName;
			}

			if (pol.PolicyId > 0)
			{
				var existing = _context.CustomerPolicies.Find(pol.PolicyId);
				if (existing != null)
					_context.Entry(existing).CurrentValues.SetValues(pol);
			}
			else
			{
				_context.CustomerPolicies.Add(pol);
			}

			_context.SaveChanges();

			return RedirectToAction("Billing");
		}

		// ================= STEP 5: BILLING =================
		[HttpGet]
		public IActionResult Billing()
		{
			var cid = GetCustomerIdOrThrow();

			ViewBag.CustomerData = GetCustomerFromSession();
			ViewBag.VehicleData = GetVehicleFromSession();
			ViewBag.EstimateData = GetEstimateFromSession();

			ViewBag.BillNumber = $"BILL-{DateTime.Now:yyMMdd}-{cid:D4}";
			ViewBag.PolicyNumber = $"POL-{DateTime.Now:yyyy}-{cid:D6}";

			var bill = _context.CustomerBillings.FirstOrDefault(b => b.CustomerId == cid);

			return View(bill ?? new CustomerBilling { CustomerId = cid });
		}

		[HttpPost]
		public async Task<IActionResult> Billing(CustomerBilling bill)
		{
			var cid = GetCustomerIdOrThrow();
			bill.CustomerId = cid;

			if (!ModelState.IsValid)
			{
				ViewBag.CustomerData = GetCustomerFromSession();
				ViewBag.VehicleData = GetVehicleFromSession();
				ViewBag.EstimateData = GetEstimateFromSession();
				return View(bill);
			}

			// FILE UPLOAD
			if (bill.ProofFile != null)
			{
				string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
				if (!Directory.Exists(folder))
					Directory.CreateDirectory(folder);

				string fileName = Guid.NewGuid() + Path.GetExtension(bill.ProofFile.FileName);
				string path = Path.Combine(folder, fileName);

				using (var stream = new FileStream(path, FileMode.Create))
				{
					await bill.ProofFile.CopyToAsync(stream);
				}

				bill.CustomerAddProve = "/uploads/" + fileName;
			}

			if (bill.BillingId > 0)
			{
				var existing = await _context.CustomerBillings.FindAsync(bill.BillingId);
				if (existing != null)
					_context.Entry(existing).CurrentValues.SetValues(bill);
			}
			else
			{
				await _context.CustomerBillings.AddAsync(bill);
			}

			await _context.SaveChangesAsync();

			return RedirectToAction("Confirmation", "Report");
		}

		// ================= SESSION HELPERS =================
		private void StoreCustomerInSession(Customer c)
		{
			HttpContext.Session.SetString("CustomerData", JsonConvert.SerializeObject(c));
		}

		private Customer GetCustomerFromSession()
		{
			var data = HttpContext.Session.GetString("CustomerData");
			return string.IsNullOrEmpty(data) ? new Customer() : JsonConvert.DeserializeObject<Customer>(data);
		}

		private void StoreVehicleInSession(Vehicle v)
		{
			HttpContext.Session.SetString("VehicleData", JsonConvert.SerializeObject(v));
		}

		private Vehicle GetVehicleFromSession()
		{
			var data = HttpContext.Session.GetString("VehicleData");
			return string.IsNullOrEmpty(data) ? new Vehicle() : JsonConvert.DeserializeObject<Vehicle>(data);
		}

		private void StoreEstimateInSession(Estimate e)
		{
			HttpContext.Session.SetString("EstimateData", JsonConvert.SerializeObject(e));
		}

		private Estimate GetEstimateFromSession()
		{
			var data = HttpContext.Session.GetString("EstimateData");
			return string.IsNullOrEmpty(data) ? new Estimate() : JsonConvert.DeserializeObject<Estimate>(data);
		}
	}
}



//using InsurancePortal.Data;
//using InsurancePortal.Migrations;
//using InsurancePortal.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Formatters.Xml;
//using Newtonsoft.Json;

//namespace InsurancePortal.Controllers
//{
//	[Authorize(Roles = "User")]
//	public class CustomerController : Controller
//	{
//		private readonly InsurancePortalContext _context;

//		public CustomerController(InsurancePortalContext context)
//		{
//			_context = context;
//		}


//		[HttpGet]
//		public IActionResult Index()
//		{
//			var cid = HttpContext.Session.GetInt32("CustomerId");
//			Customer customer = null;

//			if (cid.HasValue && cid > 0)
//			{
//				customer = _context.Customers.FirstOrDefault(c => c.CustomerId == cid);
//				if (customer != null)
//				{
//					StoreCustomerInSession(customer);
//				}
//			}

//			if (customer == null)
//			{
//				customer = GetCustomerFromSession();
//			}

//			return View(customer ?? new Customer());
//		}

//		[HttpPost]
//		public IActionResult Index(Customer customer)
//		{
//			if (!ModelState.IsValid)
//			{
//				return View(customer);   
//			}
//			if (customer.CustomerId > 0)
//			{
//				var existingCustomer = _context.Customers.Find(customer.CustomerId);
//				if (existingCustomer != null)
//				{
//					_context.Entry(existingCustomer).CurrentValues.SetValues(customer);
//					_context.SaveChanges();
//				}
//			}
//			else
//			{
//				_context.Customers.Add(customer);
//				_context.SaveChanges();
//				HttpContext.Session.SetInt32("CustomerId", customer.CustomerId);
//			}

//			StoreCustomerInSession(customer);
//			TempData["msg"] = customer.CustomerId > 0 ? "Customer updated successfully!" : $"CustomerId: {customer.CustomerId}";
//			return RedirectToAction("Vehicle");
//		}

//		[HttpGet]
//		public IActionResult Vehicle()
//		{
//			ViewBag.CustomerId = HttpContext.Session.GetInt32("CustomerId");
//			var customer = GetCustomerFromSession();
//			ViewBag.CustomerData = customer;

//			var vehicleId = HttpContext.Session.GetInt32("VehicleId");
//			Vehicle vehicle = null;
//			if (vehicleId.HasValue && vehicleId > 0)
//			{
//				vehicle = _context.Vehicles.FirstOrDefault(v => v.VehicleId == vehicleId);
//				if (vehicle != null) StoreVehicleInSession(vehicle);
//			}

//			return View(vehicle ?? new Vehicle());
//		}

//		[HttpPost]
//		public IActionResult Vehicle(Vehicle vehicle)
//		{
//			if (!ModelState.IsValid)
//			{
//				var customer = GetCustomerFromSession();
//				ViewBag.CustomerData = customer;
//				ViewBag.CustomerId = HttpContext.Session.GetInt32("CustomerId");

//				return View(vehicle);   
//			}
//			var cid = HttpContext.Session.GetInt32("CustomerId");
//			vehicle.CustomerId = cid ?? 0;

//			if (vehicle.VehicleId > 0)
//			{
//				var existingVehicle = _context.Vehicles.Find(vehicle.VehicleId);
//				if (existingVehicle != null)
//				{
//					_context.Entry(existingVehicle).CurrentValues.SetValues(vehicle);
//					_context.SaveChanges();
//				}
//			}
//			else
//			{
//				_context.Vehicles.Add(vehicle);
//				_context.SaveChanges();
//				HttpContext.Session.SetInt32("VehicleId", vehicle.VehicleId);
//			}

//			StoreVehicleInSession(vehicle);
//			TempData["msg"] = vehicle.VehicleId > 0 ? "Vehicle updated successfully!" : $"VehicleId: {vehicle.VehicleId}";
//			return RedirectToAction("Estimate");
//		}

//		[HttpGet]
//		public IActionResult Estimate()
//		{
//			var customer = GetCustomerFromSession();
//			var vehicle = GetVehicleFromSession();
//			ViewBag.CustomerData = customer;
//			ViewBag.VehicleData = vehicle;


//			var cid = HttpContext.Session.GetInt32("CustomerId");
//			var existingEstimate = _context.Estimates.FirstOrDefault(e => e.CustomerId == cid);


//			ViewBag.EstimateNumber = $"EST-{DateTime.Now:yyMMdd}-{cid:D4}";

//			return View(existingEstimate ?? new Estimate());
//		}
//		[HttpPost]
//		public IActionResult Estimate(Estimate est)
//		{
//			if (!ModelState.IsValid)
//			{
//				var customer = GetCustomerFromSession();
//				var vehicle = GetVehicleFromSession();

//				ViewBag.CustomerData = customer;
//				ViewBag.VehicleData = vehicle;

//				return View(est);   
//			}
//			var cid = HttpContext.Session.GetInt32("CustomerId");
//			est.CustomerId = cid ?? 0;

//			if (est.EstimateId > 0)
//			{
//				var existing = _context.Estimates.Find(est.EstimateId);
//				if (existing != null)
//				{
//					_context.Entry(existing).CurrentValues.SetValues(est);
//					_context.SaveChanges();
//				}
//			}
//			else
//			{
//				_context.Estimates.Add(est);
//				_context.SaveChanges();
//			}


//			StoreEstimateInSession(est);

//			TempData["msg"] = "Estimate updated successfully!";
//			return RedirectToAction("Policy");
//		}


//		[HttpGet]
//		public IActionResult Policy()
//		{
//			var customer = GetCustomerFromSession();
//			var vehicle = GetVehicleFromSession();
//			var estimate = GetEstimateFromSession();  

//			ViewBag.CustomerData = customer;
//			ViewBag.VehicleData = vehicle;
//			ViewBag.EstimateData = estimate;        

//			var cid = HttpContext.Session.GetInt32("CustomerId");
//			var existingPolicy = _context.CustomerPolicies.FirstOrDefault(p => p.CustomerId == cid);
//			ViewBag.PolicyNumber = $"POL-{DateTime.Now:yyyy}-{cid:D6}";

//			return View(existingPolicy ?? new CustomerPolicy());
//		}
//		[HttpPost]
//		public IActionResult Policy(CustomerPolicy pol)
//		{
//			if (!ModelState.IsValid)
//			{
//				var customer = GetCustomerFromSession();
//				var vehicle = GetVehicleFromSession();
//				var estimate = GetEstimateFromSession();

//				ViewBag.CustomerData = customer;
//				ViewBag.VehicleData = vehicle;
//				ViewBag.EstimateData = estimate;

//				ViewBag.PolicyNumber = $"POL-{DateTime.Now:yyyy}-{HttpContext.Session.GetInt32("CustomerId"):D6}";

//				return View(pol);
//			}

//			var cid = HttpContext.Session.GetInt32("CustomerId");
//			pol.CustomerId = cid ?? 0;

//			// ================= FILE UPLOAD =================
//			if (pol.CustomerAddProveFile != null)
//			{
//				string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

//				if (!Directory.Exists(uploadsFolder))
//				{
//					Directory.CreateDirectory(uploadsFolder);
//				}

//				string fileName = Guid.NewGuid().ToString() + Path.GetExtension(pol.CustomerAddProveFile.FileName);
//				string filePath = Path.Combine(uploadsFolder, fileName);

//				using (var stream = new FileStream(filePath, FileMode.Create))
//				{
//					pol.CustomerAddProveFile.CopyTo(stream);
//				}

//				pol.CustomerAddProvePath = "/uploads/" + fileName;
//			}

//			// ================= SAVE =================
//			if (pol.PolicyId > 0)
//			{
//				var existing = _context.CustomerPolicies.Find(pol.PolicyId);
//				if (existing != null)
//				{
//					_context.Entry(existing).CurrentValues.SetValues(pol);
//					_context.SaveChanges();
//				}
//			}
//			else
//			{
//				_context.CustomerPolicies.Add(pol);
//				_context.SaveChanges();
//			}

//			TempData["msg"] = "Policy updated successfully!";
//			return RedirectToAction("Billing");
//		}


//		[HttpGet]
//		public IActionResult Billing()
//		{
//			var customer = GetCustomerFromSession();
//			var vehicle = GetVehicleFromSession();
//			var estimate = GetEstimateFromSession();  

//			ViewBag.CustomerData = customer;
//			ViewBag.VehicleData = vehicle;
//			ViewBag.EstimateData = estimate;         

//			var cid = HttpContext.Session.GetInt32("CustomerId") ?? 0;
//			var existingBilling = _context.CustomerBillings.FirstOrDefault(b => b.CustomerId == cid);
//			ViewBag.BillNumber = $"BILL-{DateTime.Now:yyMMdd}-{cid:D4}";
//			ViewBag.PolicyNumber = $"POL-{DateTime.Now:yyyy}-{cid:D6}";

//			return View(existingBilling ?? new CustomerBilling());
//		}

//		[HttpPost]
//		public async Task<IActionResult> Billing(CustomerBilling bill)
//		{
//			var cid = HttpContext.Session.GetInt32("CustomerId");


//			if (bill.ProofFile != null)
//			{
//				string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

//				if (!Directory.Exists(folder))
//					Directory.CreateDirectory(folder);

//				string fileName = Guid.NewGuid().ToString() + Path.GetExtension(bill.ProofFile.FileName);
//				string filePath = Path.Combine(folder, fileName);

//				using (var stream = new FileStream(filePath, FileMode.Create))
//				{
//					await bill.ProofFile.CopyToAsync(stream);
//				}

//				bill.CustomerAddProve = "/uploads/" + fileName;
//			}

//			bill.CustomerId = cid ?? 0;

//			if (bill.BillingId > 0)
//			{
//				var existing = await _context.CustomerBillings.FindAsync(bill.BillingId);
//				if (existing != null)
//				{
//					_context.Entry(existing).CurrentValues.SetValues(bill);
//				}
//			}
//			else
//			{
//				await _context.CustomerBillings.AddAsync(bill);
//			}

//			await _context.SaveChangesAsync();


//			HttpContext.Session.SetObject("CustomerData", GetCustomerFromSession());
//			HttpContext.Session.SetObject("VehicleData", GetVehicleFromSession());
//			HttpContext.Session.SetObject("EstimateData", GetEstimateFromSession());

//			return RedirectToAction("Confirmation", "Report"); 
//		}

//		[HttpPost]
//		public IActionResult Confirm()
//		{
//			TempData["Success"] = "Your insurance policy has been successfully created!";
//			HttpContext.Session.Clear();
//			return Json(new { success = true, message = "Policy confirmed successfully!" });
//		}




//		private void StoreCustomerInSession(Customer customer)
//		{
//			HttpContext.Session.SetString("CustomerData", JsonConvert.SerializeObject(customer));
//		}

//		private Customer GetCustomerFromSession()
//		{
//			var customerJson = HttpContext.Session.GetString("CustomerData");
//			return string.IsNullOrEmpty(customerJson) ? new Customer() : JsonConvert.DeserializeObject<Customer>(customerJson);
//		}

//		private void StoreVehicleInSession(Vehicle vehicle)
//		{
//			HttpContext.Session.SetString("VehicleData", JsonConvert.SerializeObject(vehicle));
//		}

//		private Vehicle GetVehicleFromSession()
//		{
//			var vehicleJson = HttpContext.Session.GetString("VehicleData");
//			return string.IsNullOrEmpty(vehicleJson) ? new Vehicle() : JsonConvert.DeserializeObject<Vehicle>(vehicleJson);
//		}


//		private void StoreEstimateInSession(Estimate estimate)
//		{
//			HttpContext.Session.SetString("EstimateData", JsonConvert.SerializeObject(estimate));
//		}

//		private Estimate GetEstimateFromSession()
//		{
//			var json = HttpContext.Session.GetString("EstimateData");
//			return string.IsNullOrEmpty(json) ? new Estimate() : JsonConvert.DeserializeObject<Estimate>(json);
//		}
//	}
//}








