using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using InsurancePortal.Models;
using InsurancePortal.Controllers;

namespace InsurancePortal.Data
{
	public class InsurancePortalContext : IdentityDbContext<Users>
	{
		public InsurancePortalContext(DbContextOptions<InsurancePortalContext> options)
			: base(options)
		{
		}
		public DbSet<Users> Users { get; set; }
		public DbSet<Contact> Contacts { get; set; }
		// ✅ STEP 1: Customer Table
		public DbSet<Customer> Customers { get; set; }

		// ✅ STEP 2: Vehicle Table
		public DbSet<Vehicle> Vehicles { get; set; }

		// ✅ STEP 3: Estimate Table
		public DbSet<Estimate> Estimates { get; set; }

		// ✅ STEP 4: Policy Table
		public DbSet<CustomerPolicy> CustomerPolicies { get; set; }

		// ✅ STEP 5: Billing Table
		public DbSet<CustomerBilling> CustomerBillings { get; set; }

		// ✅ Additional Tables (Module 8, 9, 10)
		public DbSet<CompanyExpense> CompanyExpenses { get; set; }
		public DbSet<ClaimDetail> ClaimDetails { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// ✅ Customer Table Configuration
			modelBuilder.Entity<Customer>(entity =>
			{
				entity.HasKey(e => e.CustomerId);
				entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(100);
				entity.Property(e => e.CustomerAddress).IsRequired().HasMaxLength(200);
				entity.Property(e => e.CustomerPhoneNumber).IsRequired().HasMaxLength(15);
			});

			// ✅ Vehicle Table Configuration
			modelBuilder.Entity<Vehicle>(entity =>
			{
				entity.HasKey(e => e.VehicleId);
				entity.Property(e => e.VehicleName).IsRequired().HasMaxLength(100);
				entity.Property(e => e.VehicleOwnerName).IsRequired().HasMaxLength(100);
				entity.Property(e => e.VehicleModel).IsRequired().HasMaxLength(50);
				entity.Property(e => e.VehicleVersion).HasMaxLength(50);
				entity.Property(e => e.VehicleRate).IsRequired().HasColumnType("decimal(18,2)");
				entity.Property(e => e.VehicleBodyNumber).IsRequired().HasMaxLength(50);
				entity.Property(e => e.VehicleEngineNumber).IsRequired().HasMaxLength(50);
				entity.Property(e => e.VehicleNumber).IsRequired().HasMaxLength(20);

				// ✅ Foreign Key Relationship
				//	entity.HasOne(d => d.Customer)
				//		  .WithMany()
				//		  .HasForeignKey(d => d.CustomerId)
				//		  .OnDelete(DeleteBehavior.Restrict);
				//});
			});


			// ✅ Estimate Table Configuration
			modelBuilder.Entity<Estimate>(entity =>
				{
					entity.HasKey(e => e.EstimateId);
					entity.Property(e => e.CustomerId).IsRequired();
					entity.Property(e => e.EstimateNumber).IsRequired();
					entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(100);
					entity.Property(e => e.CustomerPhoneNumber).IsRequired().HasMaxLength(15);
					entity.Property(e => e.VehicleName).IsRequired().HasMaxLength(100);
					entity.Property(e => e.VehicleModel).IsRequired().HasMaxLength(50);
					entity.Property(e => e.VehicleRate).IsRequired().HasColumnType("decimal(18,2)");
					entity.Property(e => e.VehicleWarranty).HasMaxLength(100);
					entity.Property(e => e.VehiclePolicyType).IsRequired().HasMaxLength(50);
				});

			// ✅ CustomerPolicy Table Configuration
			modelBuilder.Entity<CustomerPolicy>(entity =>
			{
				entity.HasKey(e => e.PolicyId);
				entity.Property(e => e.CustomerId).IsRequired();
				entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(100);
				entity.Property(e => e.CustomerAddress).IsRequired().HasMaxLength(200);
				entity.Property(e => e.CustomerPhoneNumber).IsRequired().HasMaxLength(15);
				entity.Property(e => e.PolicyNumber).IsRequired();
				entity.Property(e => e.PolicyDate).IsRequired().HasMaxLength(50);
				entity.Property(e => e.PolicyDuration).IsRequired();
				entity.Property(e => e.VehicleNumber).IsRequired().HasMaxLength(20);
				entity.Property(e => e.VehicleName).IsRequired().HasMaxLength(100);
				entity.Property(e => e.VehicleModel).IsRequired().HasMaxLength(50);
				entity.Property(e => e.VehicleVersion).HasMaxLength(50);
				entity.Property(e => e.VehicleRate).IsRequired().HasColumnType("decimal(18,2)");
				entity.Property(e => e.VehicleWarranty).HasMaxLength(100);
				entity.Property(e => e.VehicleBodyNumber).IsRequired().HasMaxLength(50);
				entity.Property(e => e.VehicleEngineNumber).IsRequired().HasMaxLength(50);
				entity.Property(e => e.CustomerAddProvePath).HasMaxLength(200);
			});

			// ✅ CustomerBilling Table Configuration
			modelBuilder.Entity<CustomerBilling>(entity =>
			{
				entity.HasKey(e => e.BillingId);
				entity.Property(e => e.CustomerId).IsRequired();
				entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(100);
				entity.Property(e => e.PolicyNumber).IsRequired();
				entity.Property(e => e.CustomerAddProve).HasMaxLength(200);
				entity.Property(e => e.CustomerPhoneNumber).IsRequired().HasMaxLength(15);
				entity.Property(e => e.BillNo).IsRequired();
				entity.Property(e => e.VehicleName).IsRequired().HasMaxLength(100);
				entity.Property(e => e.VehicleModel).IsRequired().HasMaxLength(50);
				entity.Property(e => e.VehicleRate).IsRequired().HasColumnType("decimal(18,2)");
				entity.Property(e => e.VehicleBodyNumber).IsRequired().HasMaxLength(50);
				entity.Property(e => e.VehicleEngineNumber).IsRequired().HasMaxLength(50);
				entity.Property(e => e.Date).IsRequired().HasMaxLength(50);
				entity.Property(e => e.Amount).IsRequired().HasColumnType("decimal(18,2)");
			});

			// ✅ CompanyExpense Table Configuration
			modelBuilder.Entity<CompanyExpense>(entity =>
			{
				entity.HasKey(e => e.ExpenseId);
				entity.Property(e => e.DateOfExpense).IsRequired().HasMaxLength(50);
				entity.Property(e => e.TypeOfExpense).IsRequired().HasMaxLength(100);
				entity.Property(e => e.AmountOfExpense).IsRequired().HasColumnType("decimal(18,2)");
			});

			// ✅ ClaimDetail Table Configuration
			modelBuilder.Entity<ClaimDetail>(entity =>
			{
				entity.HasKey(e => e.ClaimNumber);
				entity.Property(e => e.PolicyNumber).IsRequired();
				entity.Property(e => e.PolicyStartDate).IsRequired().HasMaxLength(50);
				entity.Property(e => e.PolicyEndDate).IsRequired().HasMaxLength(50);
				entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(100);
				entity.Property(e => e.PlaceOfAccident).HasMaxLength(200);
				entity.Property(e => e.DateOfAccident).IsRequired().HasMaxLength(50);
				entity.Property(e => e.InsuredAmount).HasColumnType("decimal(18,2)");
				entity.Property(e => e.ClaimableAmount).HasColumnType("decimal(18,2)");
			});
		}
	}
}



