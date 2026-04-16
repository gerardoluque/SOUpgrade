using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class AtencionMedica_8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicamentoClasificaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicamentoClasificaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicamentoPresentaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicamentoPresentaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicamentoTipoArticulos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicamentoTipoArticulos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicamentoTipoControles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicamentoTipoControles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicamentoTipoDocumentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicamentoTipoDocumentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicamentoUnidadMedidas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicamentoUnidadMedidas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicamentoClasificaciones");

            migrationBuilder.DropTable(
                name: "MedicamentoPresentaciones");

            migrationBuilder.DropTable(
                name: "MedicamentoTipoArticulos");

            migrationBuilder.DropTable(
                name: "MedicamentoTipoControles");

            migrationBuilder.DropTable(
                name: "MedicamentoTipoDocumentos");

            migrationBuilder.DropTable(
                name: "MedicamentoUnidadMedidas");
        }
    }
}
