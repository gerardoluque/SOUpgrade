using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class ExpedienteMedico_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpedientesMedicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PacienteId = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreacionId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ClinicaCreacionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpedientesMedicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpedientesMedicos_ClinicasMedicas_ClinicaCreacionId",
                        column: x => x.ClinicaCreacionId,
                        principalTable: "ClinicasMedicas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpedientesMedicos_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExpedientesMedicoAtenciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpedienteMedicoId = table.Column<int>(type: "int", nullable: false),
                    MedicoId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    EspecialidadId = table.Column<int>(type: "int", nullable: false),
                    ClinicaId = table.Column<int>(type: "int", nullable: false),
                    AtencionId = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreacionId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    UsuarioModificoId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    AtencionJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpedientesMedicoAtenciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpedientesMedicoAtenciones_AtencionesMedicas_AtencionId",
                        column: x => x.AtencionId,
                        principalTable: "AtencionesMedicas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpedientesMedicoAtenciones_ClinicasMedicas_ClinicaId",
                        column: x => x.ClinicaId,
                        principalTable: "ClinicasMedicas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpedientesMedicoAtenciones_Especialidades_EspecialidadId",
                        column: x => x.EspecialidadId,
                        principalTable: "Especialidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpedientesMedicoAtenciones_ExpedientesMedicos_ExpedienteMedicoId",
                        column: x => x.ExpedienteMedicoId,
                        principalTable: "ExpedientesMedicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExpedientesMedicoDetalleBitacoras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpedienteMedicoAtencionId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    OriginalJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModificadoJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpedientesMedicoDetalleBitacoras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpedientesMedicoDetalleBitacoras_ExpedientesMedicoAtenciones_ExpedienteMedicoAtencionId",
                        column: x => x.ExpedienteMedicoAtencionId,
                        principalTable: "ExpedientesMedicoAtenciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpedientesMedicoAtenciones_AtencionId",
                table: "ExpedientesMedicoAtenciones",
                column: "AtencionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpedientesMedicoAtenciones_ClinicaId",
                table: "ExpedientesMedicoAtenciones",
                column: "ClinicaId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpedientesMedicoAtenciones_EspecialidadId",
                table: "ExpedientesMedicoAtenciones",
                column: "EspecialidadId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpedientesMedicoAtenciones_ExpedienteMedicoId_FechaCreacion",
                table: "ExpedientesMedicoAtenciones",
                columns: new[] { "ExpedienteMedicoId", "FechaCreacion" });

            migrationBuilder.CreateIndex(
                name: "IX_ExpedientesMedicoDetalleBitacoras_ExpedienteMedicoAtencionId",
                table: "ExpedientesMedicoDetalleBitacoras",
                column: "ExpedienteMedicoAtencionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpedientesMedicos_ClinicaCreacionId",
                table: "ExpedientesMedicos",
                column: "ClinicaCreacionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpedientesMedicos_PacienteId",
                table: "ExpedientesMedicos",
                column: "PacienteId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpedientesMedicoDetalleBitacoras");

            migrationBuilder.DropTable(
                name: "ExpedientesMedicoAtenciones");

            migrationBuilder.DropTable(
                name: "ExpedientesMedicos");
        }
    }
}
