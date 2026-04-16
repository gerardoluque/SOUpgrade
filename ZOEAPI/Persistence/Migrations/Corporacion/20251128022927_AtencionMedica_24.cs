using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class AtencionMedica_24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Farmacias_ClinicasMedicas_ClinicaId",
                table: "Farmacias");

            migrationBuilder.AddColumn<bool>(
                name: "ElementoInactivo",
                table: "ExpedientesMedicoAtenciones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PresupuestosFarmacia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FarmaciaId = table.Column<int>(type: "int", nullable: false),
                    Ejercicio = table.Column<int>(type: "int", nullable: false),
                    MontoPresupuesto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresupuestosFarmacia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PresupuestosFarmacia_Farmacias_FarmaciaId",
                        column: x => x.FarmaciaId,
                        principalTable: "Farmacias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PresupuestosFarmacia_FarmaciaId",
                table: "PresupuestosFarmacia",
                column: "FarmaciaId");

            migrationBuilder.CreateIndex(
                name: "IX_PresupuestosFarmacia_FarmaciaId_Ejercicio",
                table: "PresupuestosFarmacia",
                columns: new[] { "FarmaciaId", "Ejercicio" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Farmacias_ClinicasMedicas_ClinicaId",
                table: "Farmacias",
                column: "ClinicaId",
                principalTable: "ClinicasMedicas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Farmacias_ClinicasMedicas_ClinicaId",
                table: "Farmacias");

            migrationBuilder.DropTable(
                name: "PresupuestosFarmacia");

            migrationBuilder.DropColumn(
                name: "ElementoInactivo",
                table: "ExpedientesMedicoAtenciones");

            migrationBuilder.AddForeignKey(
                name: "FK_Farmacias_ClinicasMedicas_ClinicaId",
                table: "Farmacias",
                column: "ClinicaId",
                principalTable: "ClinicasMedicas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
