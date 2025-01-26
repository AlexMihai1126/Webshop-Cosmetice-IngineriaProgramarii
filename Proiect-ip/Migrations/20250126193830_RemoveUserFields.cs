using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proiect_ip.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMetrics");

            migrationBuilder.DropColumn(
                name: "Adresa",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nume",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Prenume",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Adresa",
                table: "Comenzi",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Destinatar",
                table: "Comenzi",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefon",
                table: "Comenzi",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adresa",
                table: "Comenzi");

            migrationBuilder.DropColumn(
                name: "Destinatar",
                table: "Comenzi");

            migrationBuilder.DropColumn(
                name: "Telefon",
                table: "Comenzi");

            migrationBuilder.AddColumn<string>(
                name: "Adresa",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nume",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Prenume",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Proiect_ipUserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CheltuieliTotale = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 0.0m),
                    Nivel = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    UltimaActualizareNivel = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMetrics_AspNetUsers_Proiect_ipUserID",
                        column: x => x.Proiect_ipUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserMetrics_Proiect_ipUserID",
                table: "UserMetrics",
                column: "Proiect_ipUserID",
                unique: true);
        }
    }
}
