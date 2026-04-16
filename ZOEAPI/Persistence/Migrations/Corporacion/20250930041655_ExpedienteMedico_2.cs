using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class ExpedienteMedico_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AntecedentesMedicos",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TipoAntecedente = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntecedentesMedicos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiagnosticosMedicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Capitulo = table.Column<int>(type: "int", nullable: false),
                    Existencias = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiagnosticosMedicos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medicamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Existencias = table.Column<int>(type: "int", nullable: false),
                    SustanciaActiva = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TipoPresentacion = table.Column<int>(type: "int", nullable: false),
                    Concentracion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    InformacionTerapeutica = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NombreComercial = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ViaAdministracion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicamentos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosticosMedicos_Capitulo_Codigo",
                table: "DiagnosticosMedicos",
                columns: new[] { "Capitulo", "Codigo" });

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosticosMedicos_Codigo",
                table: "DiagnosticosMedicos",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosticosMedicos_Descripcion",
                table: "DiagnosticosMedicos",
                column: "Descripcion");

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_SustanciaActiva",
                table: "Medicamentos",
                column: "SustanciaActiva");

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_SustanciaActiva_TipoPresentacion",
                table: "Medicamentos",
                columns: new[] { "SustanciaActiva", "TipoPresentacion" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AntecedentesMedicos");

            migrationBuilder.DropTable(
                name: "DiagnosticosMedicos");

            migrationBuilder.DropTable(
                name: "Medicamentos");
        }
    }
}
