using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsurancePortal.Models
{
	public class CustomerBilling
	{
		[Key]
		public int BillingId { get; set; }

		[Required]
		public int CustomerId { get; set; }

		[Required(ErrorMessage = "Customer name is required")]
		[StringLength(100)]
		public string CustomerName { get; set; }

		[Required(ErrorMessage = "Policy number is required")]
		public string PolicyNumber { get; set; }

		public string? CustomerAddProve { get; set; }

		[NotMapped]
		public IFormFile? ProofFile { get; set; }

		[Required(ErrorMessage = "Phone number is required")]
		[Phone]
		[StringLength(15)]
		public string CustomerPhoneNumber { get; set; }

		[Required(ErrorMessage = "Bill number is required")]
		public string BillNo { get; set; }

		[Required(ErrorMessage = "Vehicle name is required")]
		[StringLength(100)]
		public string VehicleName { get; set; }

		[Required(ErrorMessage = "Vehicle model is required")]
		[StringLength(50)]
		public string VehicleModel { get; set; }

		[Required(ErrorMessage = "Vehicle rate is required")]
		[Range(0, double.MaxValue, ErrorMessage = "Rate must be greater than 0")]
		public decimal VehicleRate { get; set; }

		[Required(ErrorMessage = "Vehicle body number is required")]
		[StringLength(50)]
		public string VehicleBodyNumber { get; set; }

		[Required(ErrorMessage = "Vehicle engine number is required")]
		[StringLength(50)]
		public string VehicleEngineNumber { get; set; }

		[Required(ErrorMessage = "Date is required")]
		[StringLength(50)]
		public string Date { get; set; }

		[Required(ErrorMessage = "Amount is required")]
		[Range(0, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
		public decimal Amount { get; set; }

		// Optional Foreign Key
		public int? PolicyId { get; set; }

	    

		public Customer? Customer { get; set; }
	}
}