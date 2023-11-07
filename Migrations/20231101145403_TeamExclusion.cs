

#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace UHCAPI.Migrations
{
    /// <inheritdoc />
    public partial class TeamExclusion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Teams_TeamId",
                table: "Connections");
            migrationBuilder.DropIndex(
                name: "IX_Connections_TeamId",
                table: "Connections");
            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Connections");
            migrationBuilder.DropTable(
                name: "Teams");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Color = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });
            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Connections",
                nullable: false);
            migrationBuilder.AddForeignKey(
                    name: "FK_Connections_Teams_TeamId",
                    table: "Connections",
                    column: "TeamId",
                    principalTable: "Teams",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            migrationBuilder.CreateIndex(
                name: "IX_Connections_TeamId",
                table: "Connections",
                column: "TeamId");
        }
    }
}
