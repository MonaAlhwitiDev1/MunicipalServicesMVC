using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MunicipalServicesMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceDepartmentRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_DepartmentId",
                table: "Services",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Departments_DepartmentId",
                table: "Services",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Departments_DepartmentId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_DepartmentId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Services");
        }
    }
}
