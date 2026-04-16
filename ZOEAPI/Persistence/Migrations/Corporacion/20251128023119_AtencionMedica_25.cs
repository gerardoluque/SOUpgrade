using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class AtencionMedica_25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Crear esquema Reportes
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Reportes')
                BEGIN
                    EXEC('CREATE SCHEMA Reportes')
                END
            ");

            // Crear vistas - copiar contenido del script SQL aquí
            // O mejor aún, ejecutar el script desde un archivo:
            migrationBuilder.Sql(System.IO.File.ReadAllText(
                @"..\Database\Scripts\Reportes\01_CreateSchema_And_Views.sql"
            ));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eliminar vistas en orden inverso
            migrationBuilder.Sql("DROP VIEW IF EXISTS Reportes.vw_TiempoEspera");
            migrationBuilder.Sql("DROP VIEW IF EXISTS Reportes.vw_PacientesAtendidos");
            migrationBuilder.Sql("DROP VIEW IF EXISTS Reportes.vw_AtencionesPorGenero");
            migrationBuilder.Sql("DROP VIEW IF EXISTS Reportes.vw_GastoFarmacia");
            migrationBuilder.Sql("DROP VIEW IF EXISTS Reportes.vw_MedicinaGeneral");
            migrationBuilder.Sql("DROP VIEW IF EXISTS Reportes.vw_MedicosSubrogados");
            migrationBuilder.Sql("DROP VIEW IF EXISTS Reportes.vw_RecetasExternas");
            migrationBuilder.Sql("DROP VIEW IF EXISTS Reportes.vw_MedicamentosControlados");
            migrationBuilder.Sql("DROP VIEW IF EXISTS Reportes.vw_MedicamentosInexistentes");

            // Eliminar esquema
            migrationBuilder.Sql("DROP SCHEMA IF EXISTS Reportes");
        }
    }
}
