using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MunicipalServicesMVC.Migrations
{
    public partial class AddDepartmentUID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // أولاً نضيف UID بشكل مؤقت ويقبل null
            migrationBuilder.AddColumn<string>(
                name: "UID",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: true);

            // نعطي كل إدارة قديمة UID مختلف
            migrationBuilder.Sql(
                @"UPDATE Departments
                  SET UID = CONVERT(nvarchar(36), NEWID())
                  WHERE UID IS NULL OR UID = ''"
            );

            // بعدها نخليه مطلوب ولا يقبل null
            migrationBuilder.AlterColumn<string>(
                name: "UID",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UID",
                table: "Departments");
        }
    }
}