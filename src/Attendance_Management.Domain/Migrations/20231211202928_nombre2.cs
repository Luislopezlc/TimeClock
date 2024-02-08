using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance_Management.Domain.Migrations
{
    public partial class nombre2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sidebarRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthId = table.Column<int>(type: "int", nullable: false),
                    RolId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sidebarRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sidebarRoles_AspNetRoles_RolId",
                        column: x => x.RolId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_sidebarRoles_auth_sidebar_AuthId",
                        column: x => x.AuthId,
                        principalTable: "auth_sidebar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sidebarRoles_AuthId",
                table: "sidebarRoles",
                column: "AuthId");

            migrationBuilder.CreateIndex(
                name: "IX_sidebarRoles_RolId",
                table: "sidebarRoles",
                column: "RolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sidebarRoles");
        }
    }
}
