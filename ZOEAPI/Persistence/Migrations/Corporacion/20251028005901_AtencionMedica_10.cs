using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class AtencionMedica_10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Farmacia",
                table: "Medicamentos");

            migrationBuilder.AddColumn<int>(
                name: "FarmaciaId",
                table: "Medicamentos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Farmacias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farmacias", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Farmacias");

            migrationBuilder.DropColumn(
                name: "FarmaciaId",
                table: "Medicamentos");

            migrationBuilder.AddColumn<string>(
                name: "Farmacia",
                table: "Medicamentos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
