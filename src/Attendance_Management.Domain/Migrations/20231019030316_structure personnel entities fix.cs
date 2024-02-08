using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance_Management.Domain.Migrations
{
    public partial class structurepersonnelentitiesfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployeeCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PositionId",
                table: "areasUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_areasUsers_PositionId",
                table: "areasUsers",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_areasUsers_positions_PositionId",
                table: "areasUsers",
                column: "PositionId",
                principalTable: "positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_areasUsers_positions_PositionId",
                table: "areasUsers");

            migrationBuilder.DropIndex(
                name: "IX_areasUsers_PositionId",
                table: "areasUsers");

            migrationBuilder.DropColumn(
                name: "EmployeeCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "areasUsers");
        }
    }
}
