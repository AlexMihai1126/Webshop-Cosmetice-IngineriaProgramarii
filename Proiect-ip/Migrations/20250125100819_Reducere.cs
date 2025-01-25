using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proiect_ip.Migrations
{
    /// <inheritdoc />
    public partial class Reducere : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Reducere",
                table: "Produse",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reducere",
                table: "Produse");
        }
    }
}
