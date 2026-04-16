using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class UpdateRecetasView_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW [dbo].[vw_Recetas]
                AS
                SELECT 
                    ema.Id AS ExpedienteMedicoAtencionId,
                    ema.ExpedienteMedicoId,
                    ema.AtencionId,
                    ema.MedicoId,
                    ema.ClinicaId,
                    ema.FechaCreacion,
                    CAST(receta.[key] AS INT) AS RecetaIndex,
                    emar.Folio,
                    emar.FolioClinica,
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
                    mu.Nombre AS UnidadMedida,
                    em.PacienteId,
                    CONCAT(p.Nombre, ' ', p.PrimerApellido, ' ', ISNULL(p.SegundoApellido, '')) AS PacienteNombreCompleto
                FROM dbo.ExpedientesMedicoAtenciones ema
				     LEFT JOIN
					 dbo.ExpedientesMedicoAtencionRecetas emar
					 ON emar.[ExpedienteMedicoAtencionId] = ema.Id
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
                LEFT JOIN dbo.ExpedientesMedicos em
                    ON ema.ExpedienteMedicoId = em.Id
                LEFT JOIN dbo.Pacientes p
                    ON em.PacienteId = p.Id
            WHERE emar.Folio = CAST(JSON_VALUE(receta.value, '$.Folio') AS BIGINT)
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW [dbo].[vw_Recetas]
                AS
                SELECT 
                    ema.Id AS ExpedienteMedicoAtencionId,
                    ema.ExpedienteMedicoId,
                    ema.AtencionId,
                    ema.MedicoId,
                    ema.ClinicaId,
                    ema.FechaCreacion,
                    CAST(receta.[key] AS INT) AS RecetaIndex,
                    emar.Folio,
                    emar.FolioClinica,
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
                    mu.Nombre AS UnidadMedida,
                    em.PacienteId,
                    CONCAT(p.Nombre, ' ', p.PrimerApellido, ' ', ISNULL(p.SegundoApellido, '')) AS PacienteNombreCompleto
                FROM dbo.ExpedientesMedicoAtenciones ema
				     LEFT JOIN
					 dbo.ExpedientesMedicoAtencionRecetas emar
					 ON emar.[ExpedienteMedicoAtencionId] = ema.Id
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
                LEFT JOIN dbo.ExpedientesMedicos em
                    ON ema.ExpedienteMedicoId = em.Id
                LEFT JOIN dbo.Pacientes p
                    ON em.PacienteId = p.Id
            ");
        }
    }
}
