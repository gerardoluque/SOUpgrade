using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class AtencionMedica_32 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CorporacionTenantId",
                table: "SalidasMedicamentos",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorporacionTenantId",
                table: "PresupuestosFarmacia",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorporacionTenantId",
                table: "MovimientoInventarioBitacoraEstados",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorporacionTenantId",
                table: "ExpedientesMedicos",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorporacionTenantId",
                table: "ExpedientesMedicoDetalleBitacoras",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorporacionTenantId",
                table: "ExpedientesMedicoAtencionRecetas",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorporacionTenantId",
                table: "ExpedientesMedicoAtenciones",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorporacionTenantId",
                table: "EntradasMedicamentos",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorporacionTenantId",
                table: "DetalleSalidasMedicamentosBitacora",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorporacionTenantId",
                table: "DetalleSalidasMedicamentos",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorporacionTenantId",
                table: "DetalleEntradasMedicamentos",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorporacionTenantId",
                table: "AtencionMedicaBitacoraStatuses",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorporacionTenantId",
                table: "AtencionesMedicas",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorporacionTenantId",
                table: "SalidasMedicamentos");

            migrationBuilder.DropColumn(
                name: "CorporacionTenantId",
                table: "PresupuestosFarmacia");

            migrationBuilder.DropColumn(
                name: "CorporacionTenantId",
                table: "MovimientoInventarioBitacoraEstados");

            migrationBuilder.DropColumn(
                name: "CorporacionTenantId",
                table: "ExpedientesMedicos");

            migrationBuilder.DropColumn(
                name: "CorporacionTenantId",
                table: "ExpedientesMedicoDetalleBitacoras");

            migrationBuilder.DropColumn(
                name: "CorporacionTenantId",
                table: "ExpedientesMedicoAtencionRecetas");

            migrationBuilder.DropColumn(
                name: "CorporacionTenantId",
                table: "ExpedientesMedicoAtenciones");

            migrationBuilder.DropColumn(
                name: "CorporacionTenantId",
                table: "EntradasMedicamentos");

            migrationBuilder.DropColumn(
                name: "CorporacionTenantId",
                table: "DetalleSalidasMedicamentosBitacora");

            migrationBuilder.DropColumn(
                name: "CorporacionTenantId",
                table: "DetalleSalidasMedicamentos");

            migrationBuilder.DropColumn(
                name: "CorporacionTenantId",
                table: "DetalleEntradasMedicamentos");

            migrationBuilder.DropColumn(
                name: "CorporacionTenantId",
                table: "AtencionMedicaBitacoraStatuses");

            migrationBuilder.DropColumn(
                name: "CorporacionTenantId",
                table: "AtencionesMedicas");
        }
    }
}
