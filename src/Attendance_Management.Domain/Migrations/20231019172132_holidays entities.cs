using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance_Management.Domain.Migrations
{
    public partial class holidaysentities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "att_holidays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPartial = table.Column<bool>(type: "bit", nullable: false),
                    CheckIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_att_holidays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "att_holidaysEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HolidaysId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_att_holidaysEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_att_holidaysEmployees_att_holidays_HolidaysId",
                        column: x => x.HolidaysId,
                        principalTable: "att_holidays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_att_holidaysEmployees_deparments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "deparments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_att_holidaysEmployees_DepartmentId",
                table: "att_holidaysEmployees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_att_holidaysEmployees_HolidaysId",
                table: "att_holidaysEmployees",
                column: "HolidaysId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "att_holidaysEmployees");

            migrationBuilder.DropTable(
                name: "att_holidays");
        }
    }
}
