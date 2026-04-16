using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class AddCantidadEsperadaYFechaCaptura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CantidadEsperada",
                table: "DetalleEntradasMedicamentos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCaptura",
                table: "DetalleEntradasMedicamentos",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantidadEsperada",
                table: "DetalleEntradasMedicamentos");

            migrationBuilder.DropColumn(
                name: "FechaCaptura",
                table: "DetalleEntradasMedicamentos");
        }
    }
}
