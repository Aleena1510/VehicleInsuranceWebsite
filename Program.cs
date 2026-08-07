using InsurancePortal.Data;
using InsurancePortal.Models;
using InsurancePortal.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using InsurancePortal.Models;

namespace InsurancePortal
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddDistributedMemoryCache();
			builder.Services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(30);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			builder.Services.AddDbContext<InsurancePortalContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services.AddIdentity<Users, IdentityRole>(options =>
			{
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequiredLength = 6;
				options.User.RequireUniqueEmail = true;
				options.SignIn.RequireConfirmedAccount = false;
				options.SignIn.RequireConfirmedEmail = false;
				options.SignIn.RequireConfirmedPhoneNumber = false;
			})
			 .AddEntityFrameworkStores<InsurancePortalContext>()
			   .AddDefaultTokenProviders();

			builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettigs"));
			builder.Services.AddTransient<IEmailService, EmailService>();


			var app = builder.Build();

			await SeedService.SeedDatabase(app.Services);


			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();
			app.UseSession();
			app.UseAuthorization();

			app.MapStaticAssets();

		


			
			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}")
				.WithStaticAssets();
			app.MapControllerRoute(
	              name: "admin",
	              pattern: "admin/{action=Index}/{id?}",
	            defaults: new { controller = "Admin" });


			
			app.Run();
		}
	}
}
