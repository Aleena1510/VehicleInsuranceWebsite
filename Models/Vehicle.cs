using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsurancePortal.Models
{
	public class Vehicle
	{
		[Key]
		public int VehicleId { get; set; }

		[Required(ErrorMessage = "Vehicle name is required")]
		[StringLength(100)]
		public string VehicleName { get; set; }

		[Required(ErrorMessage = "Owner name is required")]
		[StringLength(100)]
		public string VehicleOwnerName { get; set; }

		[Required(ErrorMessage = "Vehicle model is required")]
		[StringLength(50)]
		public string VehicleModel { get; set; }

		[StringLength(50)]
		public string VehicleVersion { get; set; }

		[Required(ErrorMessage = "Vehicle rate is required")]
		[Range(0, double.MaxValue, ErrorMessage = "Rate must be greater than 0")]
		public decimal VehicleRate { get; set; }

		[Required(ErrorMessage = "Vehicle body number is required")]
		[StringLength(50)]
		public string VehicleBodyNumber { get; set; }

		[Required(ErrorMessage = "Vehicle engine number is required")]
		[StringLength(50)]
		public string VehicleEngineNumber { get; set; }

		[Required(ErrorMessage = "Vehicle number is required")]
		[StringLength(20)]
		public string VehicleNumber { get; set; }

		// Foreign Key
	
		public int CustomerId { get; set; }
		
		public Customer? Customer { get; set; }

	}
}