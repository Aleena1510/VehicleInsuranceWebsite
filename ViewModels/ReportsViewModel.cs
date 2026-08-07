namespace InsurancePortal.ViewModels
{
    public class ReportsViewModel
    {
        public List<MonthlySalesReport> MonthlySales { get; set; }
        public List<VehicleAnalysis> VehicleAnalysis { get; set; }
        public List<ClaimReport> ClaimsReport { get; set; }
        public List<PolicyDueRenewal> DueRenewals { get; set; }
        public List<LapsedPolicy> LapsedPolicies { get; set; }
    }

    public class MonthlySalesReport
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalSales { get; set; }
        public int PolicyCount { get; set; }
    }

    public class VehicleAnalysis
    {
        public string VehicleNumber { get; set; }
        public int PolicyCount { get; set; }
        public decimal TotalPremium { get; set; }
    }

    public class ClaimReport
    {
        public string PolicyNumber { get; set; }
        public decimal ClaimAmount { get; set; }
        public string Status { get; set; }
    }

    public class PolicyDueRenewal
    {
        public string PolicyNumber { get; set; }
        public DateTime EndDate { get; set; }
        public string CustomerName { get; set; }
    }

    public class LapsedPolicy
    {
        public string PolicyNumber { get; set; }
        public DateTime EndDate { get; set; }
        public string CustomerName { get; set; }
    }
}
