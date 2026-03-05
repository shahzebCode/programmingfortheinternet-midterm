using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentService.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPaidToPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Payments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Payments");
        }
    }
}
