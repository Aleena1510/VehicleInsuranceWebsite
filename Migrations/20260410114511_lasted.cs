using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsurancePortal.Migrations
{
    /// <inheritdoc />
    public partial class lasted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerAddProve",
                table: "CustomerPolicies");

            migrationBuilder.AddColumn<string>(
                name: "CustomerAddProvePath",
                table: "CustomerPolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerAddProvePath",
                table: "CustomerPolicies");

            migrationBuilder.AddColumn<string>(
                name: "CustomerAddProve",
                table: "CustomerPolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
