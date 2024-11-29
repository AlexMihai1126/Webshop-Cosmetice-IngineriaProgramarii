using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proiect_ip.Migrations
{
    /// <inheritdoc />
    public partial class Fix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Decscriere",
                table: "CategoriiProduse",
                newName: "Descriere");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Produse",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "ImageType",
                table: "Produse",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Produse");

            migrationBuilder.DropColumn(
                name: "ImageType",
                table: "Produse");

            migrationBuilder.RenameColumn(
                name: "Descriere",
                table: "CategoriiProduse",
                newName: "Decscriere");
        }
    }
}
