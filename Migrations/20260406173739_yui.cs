using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsurancePortal.Migrations
{
    /// <inheritdoc />
    public partial class yui : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerBillings_Customers_CustomerId",
                table: "CustomerBillings");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPolicies_Customers_CustomerId",
                table: "CustomerPolicies");

            migrationBuilder.DropForeignKey(
                name: "FK_Estimates_Customers_CustomerId",
                table: "Estimates");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Customers_CustomerId1",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_CustomerId1",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Estimates_CustomerId",
                table: "Estimates");

            migrationBuilder.DropIndex(
                name: "IX_CustomerPolicies_CustomerId",
                table: "CustomerPolicies");

            migrationBuilder.DropIndex(
                name: "IX_CustomerBillings_CustomerId",
                table: "CustomerBillings");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "Vehicles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId1",
                table: "Vehicles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CustomerId1",
                table: "Vehicles",
                column: "CustomerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Estimates_CustomerId",
                table: "Estimates",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPolicies_CustomerId",
                table: "CustomerPolicies",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerBillings_CustomerId",
                table: "CustomerBillings",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerBillings_Customers_CustomerId",
                table: "CustomerBillings",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPolicies_Customers_CustomerId",
                table: "CustomerPolicies",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Estimates_Customers_CustomerId",
                table: "Estimates",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Customers_CustomerId1",
                table: "Vehicles",
                column: "CustomerId1",
                principalTable: "Customers",
                principalColumn: "CustomerId");
        }
    }
}
