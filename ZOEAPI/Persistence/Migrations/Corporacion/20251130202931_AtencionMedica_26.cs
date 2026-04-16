using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class AtencionMedica_26 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DetalleSalidasMedicamentosBitacora",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalidaMedicamentoId = table.Column<int>(type: "int", nullable: false),
                    DetalleSalidaMedicamentoId = table.Column<int>(type: "int", nullable: false),
                    TipoMovimiento = table.Column<int>(type: "int", nullable: false),
                    MedicamentoId = table.Column<int>(type: "int", nullable: false),
                    CantidadEntregadaAnterior = table.Column<int>(type: "int", nullable: false),
                    CantidadEntregadaNueva = table.Column<int>(type: "int", nullable: false),
                    CantidadAnterior = table.Column<int>(type: "int", nullable: false),
                    CantidadNueva = table.Column<int>(type: "int", nullable: false),
                    EstadoSalidaAnterior = table.Column<int>(type: "int", nullable: false),
                    EstadoSalidaNuevo = table.Column<int>(type: "int", nullable: false),
                    InexistenteAnterior = table.Column<bool>(type: "bit", nullable: false),
                    InexistenteNuevo = table.Column<bool>(type: "bit", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    UsuarioNombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FechaCambio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ObservacionesAnteriores = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ObservacionesNuevas = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleSalidasMedicamentosBitacora", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetalleSalidasMedicamentosBitacora");
        }
    }
}
