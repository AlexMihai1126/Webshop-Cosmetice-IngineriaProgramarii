using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proiect_ip.Migrations
{
    /// <inheritdoc />
    public partial class TestDB2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comenzi_AspNetUsers_Proiect_ipUserID",
                table: "Comenzi");

            migrationBuilder.DropForeignKey(
                name: "FK_Produse_CategoriiProduse_IdCategorie",
                table: "Produse");

            migrationBuilder.AlterColumn<int>(
                name: "IdCategorie",
                table: "Produse",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Comenzi_AspNetUsers_Proiect_ipUserID",
                table: "Comenzi",
                column: "Proiect_ipUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Produse_CategoriiProduse_IdCategorie",
                table: "Produse",
                column: "IdCategorie",
                principalTable: "CategoriiProduse",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comenzi_AspNetUsers_Proiect_ipUserID",
                table: "Comenzi");

            migrationBuilder.DropForeignKey(
                name: "FK_Produse_CategoriiProduse_IdCategorie",
                table: "Produse");

            migrationBuilder.AlterColumn<int>(
                name: "IdCategorie",
                table: "Produse",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comenzi_AspNetUsers_Proiect_ipUserID",
                table: "Comenzi",
                column: "Proiect_ipUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Produse_CategoriiProduse_IdCategorie",
                table: "Produse",
                column: "IdCategorie",
                principalTable: "CategoriiProduse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
