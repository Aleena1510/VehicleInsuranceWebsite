using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsurancePortal.Models
{
	public class Estimate
	{
		[Key]
		public int EstimateId { get; set; }

		[Required]
		public int CustomerId { get; set; }

		[Required(ErrorMessage = "Estimate number is required")]
		public string EstimateNumber { get; set; }

		[Required(ErrorMessage = "Customer name is required")]
		[StringLength(100)]
		public string CustomerName { get; set; }

		[Required(ErrorMessage = "Phone number is required")]
		[Phone]
		[StringLength(15)]
		public string CustomerPhoneNumber { get; set; }

		[Required(ErrorMessage = "Vehicle name is required")]
		[StringLength(100)]
		public string VehicleName { get; set; }

		[Required(ErrorMessage = "Vehicle model is required")]
		[StringLength(50)]
		public string VehicleModel { get; set; }

		[Required(ErrorMessage = "Vehicle rate is required")]
		[Range(0, double.MaxValue, ErrorMessage = "Rate must be greater than 0")]
		public decimal VehicleRate { get; set; }

		[StringLength(100)]
		public string VehicleWarranty { get; set; }

		[Required(ErrorMessage = "Policy type is required")]
		[StringLength(50)]
		public string VehiclePolicyType { get; set; }

		// Optional Foreign Key (if you want to link)
		[ForeignKey("Vehicle")]
		
		
		public int? VehicleId { get; set; }

		

		public Customer? Customer { get; set; }

	}
}