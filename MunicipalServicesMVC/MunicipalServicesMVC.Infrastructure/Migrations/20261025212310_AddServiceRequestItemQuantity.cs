using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MunicipalServicesMVC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceRequestItemQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ServiceRequestItems",
                type: "int",
                nullable: false,
               defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ServiceRequestItems");
        }
    }
}