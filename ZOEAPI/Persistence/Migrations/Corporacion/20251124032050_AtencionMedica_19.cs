using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class AtencionMedica_19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MedicoSubrogado",
                table: "ExpedientesMedicoAtenciones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PacienteFechaConstanciaEstudiosVencida",
                table: "ExpedientesMedicoAtenciones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PacienteInactivo",
                table: "ExpedientesMedicoAtenciones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_SalidasMedicamentos_PacienteId",
                table: "SalidasMedicamentos",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_SalidasMedicamentos_TipoDocumentoId",
                table: "SalidasMedicamentos",
                column: "TipoDocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntradasMedicamentos_TipoDocumentoId",
                table: "EntradasMedicamentos",
                column: "TipoDocumentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntradasMedicamentos_MedicamentoTipoDocumentos_TipoDocumentoId",
                table: "EntradasMedicamentos",
                column: "TipoDocumentoId",
                principalTable: "MedicamentoTipoDocumentos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalidasMedicamentos_MedicamentoTipoDocumentos_TipoDocumentoId",
                table: "SalidasMedicamentos",
                column: "TipoDocumentoId",
                principalTable: "MedicamentoTipoDocumentos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalidasMedicamentos_Pacientes_PacienteId",
                table: "SalidasMedicamentos",
                column: "PacienteId",
                principalTable: "Pacientes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntradasMedicamentos_MedicamentoTipoDocumentos_TipoDocumentoId",
                table: "EntradasMedicamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_SalidasMedicamentos_MedicamentoTipoDocumentos_TipoDocumentoId",
                table: "SalidasMedicamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_SalidasMedicamentos_Pacientes_PacienteId",
                table: "SalidasMedicamentos");

            migrationBuilder.DropIndex(
                name: "IX_SalidasMedicamentos_PacienteId",
                table: "SalidasMedicamentos");

            migrationBuilder.DropIndex(
                name: "IX_SalidasMedicamentos_TipoDocumentoId",
                table: "SalidasMedicamentos");

            migrationBuilder.DropIndex(
                name: "IX_EntradasMedicamentos_TipoDocumentoId",
                table: "EntradasMedicamentos");

            migrationBuilder.DropColumn(
                name: "MedicoSubrogado",
                table: "ExpedientesMedicoAtenciones");

            migrationBuilder.DropColumn(
                name: "PacienteFechaConstanciaEstudiosVencida",
                table: "ExpedientesMedicoAtenciones");

            migrationBuilder.DropColumn(
                name: "PacienteInactivo",
                table: "ExpedientesMedicoAtenciones");
        }
    }
}
