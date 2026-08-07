using System.ComponentModel.DataAnnotations;
namespace InsurancePortal.Models
{
	public class Customer
	{
		[Key]
		public int CustomerId { get; set; }

		[Required(ErrorMessage = "Customer name is required")]
		[StringLength(100)]
		[RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only alphabets allowed")]
		public string CustomerName { get; set; }

		[Required(ErrorMessage = "Customer address is required")]
		[StringLength(200)]
		public string CustomerAddress { get; set; }

		[Required(ErrorMessage = "Phone number is required")]
		[RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be exactly 11 digits")]
		public string CustomerPhoneNumber { get; set; }
	}
}


//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace InsurancePortal.Models
//{
//	public class Customer
//	{
//		[Key]
//		public int CustomerId { get; set; }

//		[Required(ErrorMessage = "Customer name is required")]
//		[StringLength(100)]
//		public string CustomerName { get; set; }

//		[Required(ErrorMessage = "Customer address is required")]
//		[StringLength(200)]
//		public string CustomerAddress { get; set; }

//		[Required(ErrorMessage = "Phone number is required")]
//		[Phone]
//		[StringLength(15)]
//		public string CustomerPhoneNumber { get; set; }




//	}
//}