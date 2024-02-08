using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance_Management.Domain.Migrations
{
    public partial class structurepersonnelentitiesfix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeCode",
                table: "deparments");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "deparments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsLeader",
                table: "areasUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_deparments_UserId",
                table: "deparments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_deparments_AspNetUsers_UserId",
                table: "deparments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_deparments_AspNetUsers_UserId",
                table: "deparments");

            migrationBuilder.DropIndex(
                name: "IX_deparments_UserId",
                table: "deparments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "deparments");

            migrationBuilder.DropColumn(
                name: "IsLeader",
                table: "areasUsers");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeCode",
                table: "deparments",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }
    }
}
