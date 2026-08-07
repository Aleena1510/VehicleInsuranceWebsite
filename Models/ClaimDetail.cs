using System.ComponentModel.DataAnnotations;

namespace InsurancePortal.Models
{
	public class ClaimDetail
	{
		[Key]
		public int ClaimNumber { get; set; }

		[Required(ErrorMessage = "Policy number is required")]
		public int PolicyNumber { get; set; }

		[Required(ErrorMessage = "Policy start date is required")]
		[StringLength(50)]
		public string PolicyStartDate { get; set; }

		[Required(ErrorMessage = "Policy end date is required")]
		[StringLength(50)]
		public string PolicyEndDate { get; set; }

		[Required(ErrorMessage = "Customer name is required")]
		[StringLength(100)]
		public string CustomerName { get; set; }

		[StringLength(200)]
		public string PlaceOfAccident { get; set; }

		[Required(ErrorMessage = "Date of accident is required")]
		[StringLength(50)]
		public string DateOfAccident { get; set; }

		[Range(0, double.MaxValue, ErrorMessage = "Insured amount must be greater than 0")]
		public decimal InsuredAmount { get; set; }

		[Range(0, double.MaxValue, ErrorMessage = "Claimable amount must be greater than 0")]
		public decimal ClaimableAmount { get; set; }
	}
}