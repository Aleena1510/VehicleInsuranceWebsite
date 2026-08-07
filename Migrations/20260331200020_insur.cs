using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsurancePortal.Migrations
{
    /// <inheritdoc />
    public partial class insur : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Estimates_VehicleId",
                table: "Estimates",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estimates_Vehicles_VehicleId",
                table: "Estimates",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estimates_Vehicles_VehicleId",
                table: "Estimates");

            migrationBuilder.DropIndex(
                name: "IX_Estimates_VehicleId",
                table: "Estimates");
        }
    }
}
