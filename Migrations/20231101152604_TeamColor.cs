using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UHCAPI.Migrations
{
    /// <inheritdoc />
    public partial class TeamColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamColor",
                table: "Connections",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamColor",
                table: "Connections");
        }
    }
}
