using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proiect_ip.Migrations
{
    /// <inheritdoc />
    public partial class updateComanda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PuncteUtilizate",
                table: "Comenzi",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Reducere",
                table: "Comenzi",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PuncteUtilizate",
                table: "Comenzi");

            migrationBuilder.DropColumn(
                name: "Reducere",
                table: "Comenzi");
        }
    }
}
