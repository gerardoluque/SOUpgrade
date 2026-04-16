using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class ExpedienteMedico_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExamenesLaboratorioMedicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Paquete = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Clave = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Examen = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    PadreId = table.Column<int>(type: "int", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamenesLaboratorioMedicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamenesLaboratorioMedicos_ExamenesLaboratorioMedicos_PadreId",
                        column: x => x.PadreId,
                        principalTable: "ExamenesLaboratorioMedicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamenesLaboratorioMedicos_Clave",
                table: "ExamenesLaboratorioMedicos",
                column: "Clave");

            migrationBuilder.CreateIndex(
                name: "IX_ExamenesLaboratorioMedicos_Clave_Examen",
                table: "ExamenesLaboratorioMedicos",
                columns: new[] { "Clave", "Examen" });

            migrationBuilder.CreateIndex(
                name: "IX_ExamenesLaboratorioMedicos_Examen",
                table: "ExamenesLaboratorioMedicos",
                column: "Examen");

            migrationBuilder.CreateIndex(
                name: "IX_ExamenesLaboratorioMedicos_PadreId",
                table: "ExamenesLaboratorioMedicos",
                column: "PadreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamenesLaboratorioMedicos");
        }
    }
}
