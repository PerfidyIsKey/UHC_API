using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UHCAPI.Migrations
{
    /// <inheritdoc />
    public partial class Stats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VictoryType",
                table: "Seasons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KilledBy",
                table: "Connections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "KilledFirst",
                table: "Connections",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "KilledLast",
                table: "Connections",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimeKilled",
                table: "Connections",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VictoryType",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "KilledBy",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "KilledFirst",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "KilledLast",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "TimeKilled",
                table: "Connections");
        }
    }
}
