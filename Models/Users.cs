using Microsoft.AspNetCore.Identity;

namespace InsurancePortal.Models
{
	public class Users: IdentityUser
	{
		public string FullName {  get; set; }
	}
}
