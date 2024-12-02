using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proiect_ip.Migrations
{
    /// <inheritdoc />
    public partial class ModifyBrand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Produse");

            migrationBuilder.AddColumn<int>(
                name: "IdBrand",
                table: "Produse",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Branduri",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeBrand = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branduri", x => x.BrandId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produse_IdBrand",
                table: "Produse",
                column: "IdBrand");

            migrationBuilder.AddForeignKey(
                name: "FK_Produse_Branduri_IdBrand",
                table: "Produse",
                column: "IdBrand",
                principalTable: "Branduri",
                principalColumn: "BrandId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produse_Branduri_IdBrand",
                table: "Produse");

            migrationBuilder.DropTable(
                name: "Branduri");

            migrationBuilder.DropIndex(
                name: "IX_Produse_IdBrand",
                table: "Produse");

            migrationBuilder.DropColumn(
                name: "IdBrand",
                table: "Produse");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Produse",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
