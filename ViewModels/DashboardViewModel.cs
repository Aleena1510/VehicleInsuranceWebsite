// Models/ViewModels/DashboardViewModel.cs
using InsurancePortal.Models;

namespace InsurancePortal.Models.ViewModels
{
	public class DashboardViewModel
	{
		public int TotalCustomers { get; set; }
		public int TotalVehicles { get; set; }
		public int TotalPolicies { get; set; }
		public decimal TotalBillings { get; set; }
		public int TotalClaims { get; set; }
		public int TotalEstimates { get; set; }
		public List<Customer> RecentCustomers { get; set; } = new();
		public List<CustomerPolicy> RecentPolicies { get; set; } = new();
		public List<MonthlyRevenue> MonthlyRevenue { get; set; } = new();
	}

	public class MonthlyRevenue
	{
		public string Month { get; set; }
		public decimal Amount { get; set; }
	}

	public class ReportViewModel
	{
		public string Title { get; set; }
		public decimal Value { get; set; }
		public int Count { get; set; }
	}
}