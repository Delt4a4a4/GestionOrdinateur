using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionOrdinateurs.API.Migrations
{
    /// <inheritdoc />
    public partial class AjouterReparationsEtCorrigerPrix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Prix",
                table: "Ordinateurs",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "Reparations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UnOrdinateurId = table.Column<int>(type: "INTEGER", nullable: false),
                    TypeReparation = table.Column<string>(type: "TEXT", nullable: false),
                    DateDebut = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Statut = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reparations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reparations_Ordinateurs_UnOrdinateurId",
                        column: x => x.UnOrdinateurId,
                        principalTable: "Ordinateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reparations_UnOrdinateurId",
                table: "Reparations",
                column: "UnOrdinateurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reparations");

            migrationBuilder.AlterColumn<decimal>(
                name: "Prix",
                table: "Ordinateurs",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");
        }
    }
}
