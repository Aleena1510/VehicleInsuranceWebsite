using System.ComponentModel.DataAnnotations;

namespace InsurancePortal.Models
{
	public class CompanyExpense
	{
		[Key]
		public int ExpenseId { get; set; }

		[Required(ErrorMessage = "Date of expense is required")]
		[StringLength(50)]
		public string DateOfExpense { get; set; }

		[Required(ErrorMessage = "Type of expense is required")]
		[StringLength(100)]
		public string TypeOfExpense { get; set; }

		[Required(ErrorMessage = "Amount is required")]
		[Range(0, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
		public decimal AmountOfExpense { get; set; }
	}
}