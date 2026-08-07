using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InsurancePortal.Data;
using InsurancePortal.Models;

namespace InsurancePortal.Services
{
	public class SeedService
	{
		public static async Task SeedDatabase(IServiceProvider serviceProvider)
		{
			using var scope=serviceProvider.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<InsurancePortalContext>();
			var roleManager = scope.ServiceProvider.GetRequiredService <RoleManager< IdentityRole>>();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Users>>();
			var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedService>>();

			try
			{
				//Ensure the database is ready
				logger.LogInformation("Esuring the database is created");
				await context.Database.EnsureCreatedAsync();

				//Add roles
				logger.LogInformation("Seeding roles.");
				await AddRoleAsync(roleManager, "Admin");
				await AddRoleAsync(roleManager, "User");

				logger.LogInformation("Seeding admin user.");
				var adminEmail = "admin@gmail.com";
				if (await userManager.FindByEmailAsync(adminEmail) == null)
				{
					var adminUser = new Users
					{
						FullName = "Code hub",
						UserName = adminEmail,
						NormalizedUserName = adminEmail,
						Email = adminEmail,
						NormalizedEmail = adminEmail.ToUpper(),
						EmailConfirmed = true,
						SecurityStamp = Guid.NewGuid().ToString(),


					};
					var result = await userManager.CreateAsync(adminUser, "Admin@123");
					if (result.Succeeded)
					{
						logger.LogInformation("Assinging Admin role to the admin user.");
						await userManager.AddToRoleAsync(adminUser, "Admin");
					}
					else
					{
						logger.LogError("Faild to create admin user:{Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
					}
					



				}
			}
			catch (Exception ex) {
				logger.LogError(ex, "An error occured while seeding the database");
			
			}


		}

		private static async Task AddRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
		{
			if (!await roleManager.RoleExistsAsync(roleName)) {

				var result = await roleManager.CreateAsync(new IdentityRole(roleName));
				if (result.Succeeded) {
					throw new Exception($"Faild to create role `{roleName}`:{string.Join(", ", result.Errors.Select(e => e.Description))}");
}
			}
		}
	}
}
