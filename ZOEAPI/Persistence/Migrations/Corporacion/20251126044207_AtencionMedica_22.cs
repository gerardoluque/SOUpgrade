using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class AtencionMedica_22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CapacidadDiaria",
                table: "ClinicasMedicas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumeroConsultorios",
                table: "ClinicasMedicas",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CapacidadDiaria",
                table: "ClinicasMedicas");

            migrationBuilder.DropColumn(
                name: "NumeroConsultorios",
                table: "ClinicasMedicas");
        }
    }
}
