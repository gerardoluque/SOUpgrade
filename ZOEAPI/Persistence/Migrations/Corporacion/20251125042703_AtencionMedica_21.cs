using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class AtencionMedica_21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovimientoInventarioBitacoraEstados",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoMovimiento = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    MovimientoId = table.Column<int>(type: "int", nullable: false),
                    EstadoAnterior = table.Column<int>(type: "int", nullable: false),
                    EstadoNuevo = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    UsuarioNombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FechaCambio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comentarios = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DetallesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientoInventarioBitacoraEstados", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoInventarioBitacoraEstados_FechaCambio",
                table: "MovimientoInventarioBitacoraEstados",
                column: "FechaCambio");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoInventarioBitacoraEstados_TipoMovimiento_MovimientoId",
                table: "MovimientoInventarioBitacoraEstados",
                columns: new[] { "TipoMovimiento", "MovimientoId" });

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoInventarioBitacoraEstados_UsuarioId",
                table: "MovimientoInventarioBitacoraEstados",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovimientoInventarioBitacoraEstados");
        }
    }
}
