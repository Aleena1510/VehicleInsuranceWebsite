using InsurancePortal.Models;

public class CustomerDetailsViewModel
{
	public Customer Customer { get; set; }
	public List<Vehicle> Vehicles { get; set; } = new();
	public List<CustomerPolicy> Policies { get; set; } = new();
	public List<CustomerBilling> Billings { get; set; } = new();
	public List<Estimate> Estimates { get; set; } = new();
}