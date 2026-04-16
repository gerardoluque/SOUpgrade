using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class AtencionMedica_18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SalidaMedicamentoId",
                table: "ExpedientesMedicoAtencionRecetas",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CantidadEsperada",
                table: "DetalleEntradasMedicamentos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalidaMedicamentoId",
                table: "ExpedientesMedicoAtencionRecetas");

            migrationBuilder.AlterColumn<int>(
                name: "CantidadEsperada",
                table: "DetalleEntradasMedicamentos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
