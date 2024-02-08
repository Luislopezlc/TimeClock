using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance_Management.Domain.Migrations
{
    public partial class addprioritysidebar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTimeBreak",
                table: "personnel_schedules");

            migrationBuilder.DropColumn(
                name: "IsMixed",
                table: "personnel_schedules");

            migrationBuilder.DropColumn(
                name: "StartTimeBreak",
                table: "personnel_schedules");

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "auth_sidebar",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "auth_sidebar");

            migrationBuilder.AddColumn<string>(
                name: "EndTimeBreak",
                table: "personnel_schedules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMixed",
                table: "personnel_schedules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StartTimeBreak",
                table: "personnel_schedules",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
