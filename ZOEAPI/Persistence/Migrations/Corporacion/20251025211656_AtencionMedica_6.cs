using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class AtencionMedica_6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PacienteConDiscapacidad",
                table: "ExpedientesMedicoAtenciones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PacienteConDiscapacidad",
                table: "AtencionesMedicas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PacienteConDiscapacidad",
                table: "ExpedientesMedicoAtenciones");

            migrationBuilder.DropColumn(
                name: "PacienteConDiscapacidad",
                table: "AtencionesMedicas");
        }
    }
}
