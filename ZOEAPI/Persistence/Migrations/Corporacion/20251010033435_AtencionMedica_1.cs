using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class AtencionMedica_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CorporacionAdscritoId",
                table: "Pacientes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PacienteCorporacionAdscritoId",
                table: "ExpedientesMedicoAtenciones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Parentesco",
                table: "AntecedentesMedicos",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorporacionAdscritoId",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "PacienteCorporacionAdscritoId",
                table: "ExpedientesMedicoAtenciones");

            migrationBuilder.DropColumn(
                name: "Parentesco",
                table: "AntecedentesMedicos");
        }
    }
}
