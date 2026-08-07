using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsurancePortal.Models
{
	public class CustomerPolicy
	{
		[Key]
		public int PolicyId { get; set; }

		[Required]
		public int CustomerId { get; set; }

		[Required(ErrorMessage = "Customer name is required")]
		[StringLength(100)]
		public string CustomerName { get; set; }

		[Required(ErrorMessage = "Customer address is required")]
		[StringLength(200)]
		public string CustomerAddress { get; set; }

		[Required(ErrorMessage = "Phone number is required")]
		[Phone]
		[StringLength(15)]
		public string CustomerPhoneNumber { get; set; }

		[Required(ErrorMessage = "Policy number is required")]
		public string PolicyNumber { get; set; }

		[Required(ErrorMessage = "Policy date is required")]
		[StringLength(50)]
		public string PolicyDate { get; set; }

		[Required(ErrorMessage = "Policy duration is required")]
		[Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than 0")]
		public int PolicyDuration { get; set; }

		[Required(ErrorMessage = "Vehicle number is required")]
		[StringLength(20)]
		public string VehicleNumber { get; set; }

		[Required(ErrorMessage = "Vehicle name is required")]
		[StringLength(100)]
		public string VehicleName { get; set; }

		[Required(ErrorMessage = "Vehicle model is required")]
		[StringLength(50)]
		public string VehicleModel { get; set; }

		[StringLength(50)]
		public string VehicleVersion { get; set; }

		[Required(ErrorMessage = "Vehicle rate is required")]
		[Range(0, double.MaxValue, ErrorMessage = "Rate must be greater than 0")]
		public decimal VehicleRate { get; set; }

		[StringLength(100)]
		public string VehicleWarranty { get; set; }

		[Required(ErrorMessage = "Vehicle body number is required")]
		[StringLength(50)]
		public string VehicleBodyNumber { get; set; }

		[Required(ErrorMessage = "Vehicle engine number is required")]
		[StringLength(50)]
		public string VehicleEngineNumber { get; set; }

		[NotMapped]
		public IFormFile CustomerAddProveFile { get; set; }  // upload file

		public string? CustomerAddProvePath { get; set; }

		// Optional Foreign Key
		public int? EstimateId { get; set; }

        public string Status { get; set; } = "Pending";
        public Customer? Customer { get; set; }
	}
}