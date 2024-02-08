using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance_Management.DataExternal.Migrations
{
    public partial class schedulesRemane : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_personnel_Schedules_att_Days_DayId",
                table: "personnel_Schedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_personnel_Schedules",
                table: "personnel_Schedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_att_Days",
                table: "att_Days");

            migrationBuilder.RenameTable(
                name: "personnel_Schedules",
                newName: "personnel_schedules");

            migrationBuilder.RenameTable(
                name: "att_Days",
                newName: "att_days");

            migrationBuilder.RenameIndex(
                name: "IX_personnel_Schedules_DayId",
                table: "personnel_schedules",
                newName: "IX_personnel_schedules_DayId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_personnel_schedules",
                table: "personnel_schedules",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_att_days",
                table: "att_days",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_personnel_schedules_att_days_DayId",
                table: "personnel_schedules",
                column: "DayId",
                principalTable: "att_days",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_personnel_schedules_att_days_DayId",
                table: "personnel_schedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_personnel_schedules",
                table: "personnel_schedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_att_days",
                table: "att_days");

            migrationBuilder.RenameTable(
                name: "personnel_schedules",
                newName: "personnel_Schedules");

            migrationBuilder.RenameTable(
                name: "att_days",
                newName: "att_Days");

            migrationBuilder.RenameIndex(
                name: "IX_personnel_schedules_DayId",
                table: "personnel_Schedules",
                newName: "IX_personnel_Schedules_DayId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_personnel_Schedules",
                table: "personnel_Schedules",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_att_Days",
                table: "att_Days",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_personnel_Schedules_att_Days_DayId",
                table: "personnel_Schedules",
                column: "DayId",
                principalTable: "att_Days",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
