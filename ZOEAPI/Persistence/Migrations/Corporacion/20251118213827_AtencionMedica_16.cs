using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class AtencionMedica_16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpedientesMedicoAtencionRecetas",
                columns: table => new
                {
                    ExpedienteMedicoAtencionId = table.Column<int>(type: "int", nullable: false),
                    Folio = table.Column<long>(type: "bigint", nullable: false),
                    FolioClinica = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpedientesMedicoAtencionRecetas", x => new { x.ExpedienteMedicoAtencionId, x.Folio });
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpedientesMedicoAtencionRecetas_FolioClinica",
                table: "ExpedientesMedicoAtencionRecetas",
                column: "FolioClinica");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpedientesMedicoAtencionRecetas");
        }
    }
}
