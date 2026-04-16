using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class AtencionMedica_23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClinicaId",
                table: "Farmacias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Farmacias_ClinicaId",
                table: "Farmacias",
                column: "ClinicaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Farmacias_ClinicasMedicas_ClinicaId",
                table: "Farmacias",
                column: "ClinicaId",
                principalTable: "ClinicasMedicas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Farmacias_ClinicasMedicas_ClinicaId",
                table: "Farmacias");

            migrationBuilder.DropIndex(
                name: "IX_Farmacias_ClinicaId",
                table: "Farmacias");

            migrationBuilder.DropColumn(
                name: "ClinicaId",
                table: "Farmacias");
        }
    }
}
