using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class CreateRecetasView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Crear vista que extrae datos del JSON RecetaJson
            migrationBuilder.Sql(@"
                CREATE VIEW dbo.vw_Recetas
                AS
                SELECT 
                    ema.Id AS ExpedienteMedicoAtencionId,
                    ema.ExpedienteMedicoId,
                    ema.AtencionId,
                    ema.MedicoId,
                    ema.ClinicaId,
                    ema.FechaCreacion,
                    CAST(receta.[key] AS INT) AS RecetaIndex,
                    CAST(JSON_VALUE(receta.value, '$.Folio') AS BIGINT) AS Folio,
                    JSON_VALUE(receta.value, '$.FolioClinica') AS FolioClinica,
                    CAST(medicamento.[key] AS INT) AS MedicamentoIndex,
                    CAST(JSON_VALUE(medicamento.value, '$.id') AS INT) AS MedicamentoId,
                    JSON_VALUE(medicamento.value, '$.sustanciaActiva') AS SustanciaActiva,
                    JSON_VALUE(medicamento.value, '$.indicacion') AS Indicacion,
                    JSON_VALUE(medicamento.value, '$.cantidad') AS Cantidad,
                    JSON_VALUE(medicamento.value, '$.tipo') AS Tipo,
                    m.Existencias,
                    m.PrecioUnitarioConIVA,
                    m.PrecioUnitarioSinIVA,
                    m.ClaveArticulo,
                    m.PresentacionId,
                    mp.Nombre AS Presentacion,
                    m.ClasificacionId,
                    mc.Nombre AS Clasificacion,
                    m.Concentracion,
                    m.DescripcionArticulo,
                    m.UnidadMedidaId,
                    mu.Nombre AS UnidadMedida 
                FROM dbo.ExpedientesMedicoAtenciones ema
                CROSS APPLY OPENJSON(ema.RecetaJson, '$.Receta') AS receta
                CROSS APPLY OPENJSON(receta.value, '$.Medicamentos') AS medicamento
                LEFT JOIN dbo.Medicamentos m 
                    ON m.Id = CAST(JSON_VALUE(medicamento.value, '$.id') AS INT)
                LEFT JOIN dbo.MedicamentoClasificaciones AS mc 
                    ON m.ClasificacionId = mc.Id
                LEFT JOIN dbo.MedicamentoPresentaciones mp 
                    ON m.PresentacionId = mp.Id
                LEFT JOIN dbo.MedicamentoUnidadMedidas mu 
                    ON m.UnidadMedidaId = mu.Id 
                WHERE ema.RecetaJson IS NOT NULL 
                    AND ema.RecetaJson <> '{}'
            ");

            // Create indexes on the base table instead to improve view performance
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ExpedientesMedicoAtenciones_RecetaJson' AND object_id = OBJECT_ID('dbo.ExpedientesMedicoAtenciones'))
                BEGIN
                    CREATE NONCLUSTERED INDEX IX_ExpedientesMedicoAtenciones_RecetaJson
                    ON dbo.ExpedientesMedicoAtenciones (Id, ClinicaId, FechaCreacion)
                    WHERE RecetaJson IS NOT NULL AND RecetaJson <> '{}'
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS dbo.vw_Recetas");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_ExpedientesMedicoAtenciones_RecetaJson ON dbo.ExpedientesMedicoAtenciones");
        }
    }
}
