using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class AddSecuenciaFolioRecetaAndUnidadPrefijo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnidadPrefijo",
                table: "ClinicasMedicas",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SecuenciasFolioReceta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClinicaId = table.Column<int>(type: "int", nullable: false),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    UltimoFolio = table.Column<long>(type: "bigint", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecuenciasFolioReceta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecuenciasFolioReceta_ClinicasMedicas_ClinicaId",
                        column: x => x.ClinicaId,
                        principalTable: "ClinicasMedicas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SecuenciasFolioReceta_ClinicaId",
                table: "SecuenciasFolioReceta",
                column: "ClinicaId");

            migrationBuilder.CreateIndex(
                name: "IX_SecuenciasFolioReceta_ClinicaId_Anio",
                table: "SecuenciasFolioReceta",
                columns: new[] { "ClinicaId", "Anio" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SecuenciasFolioReceta");

            migrationBuilder.DropColumn(
                name: "UnidadPrefijo",
                table: "ClinicasMedicas");
        }
    }
}
