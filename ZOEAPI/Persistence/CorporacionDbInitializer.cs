 using Microsoft.EntityFrameworkCore;

namespace API.Persistence
{
    public static class CorporacionDbInitializer
    {
        /// <summary>
        /// Siembra datos base por corporación. Se asegura de no duplicar datos si ya existen.
        /// </summary>
        public static async Task SeedData(CorporacionDbContext context, AppDbContext appDbContext, string? corporacionId = null)
        {
            
            

            if (!await context.ServiciosSalud.AnyAsync())
            {
                await context.ServiciosSalud.AddRangeAsync(new[]
                {
                    new Domain.Catalogos.ServicioSalud { Id = 1, Nombre = "ISSEMyM", Activo = true },
                    new Domain.Catalogos.ServicioSalud { Id = 2, Nombre = "IMSS", Activo = true },
                });
            }

            // Áreas
            if (!await context.Areas.AnyAsync())
            {
                var areas = new List<Domain.Catalogos.Area>
                {
                    new Domain.Catalogos.Area { Id = 1, Nombre = "Area 1", Activo = true },
                    new Domain.Catalogos.Area { Id = 2, Nombre = "Area 2", Activo = true },
                    new Domain.Catalogos.Area { Id = 3, Nombre = "Area 3", Activo = true },
                    new Domain.Catalogos.Area { Id = 4, Nombre = "Area 4", Activo = true },
                    new Domain.Catalogos.Area { Id = 5, Nombre = "Area 5", Activo = true },
                    new Domain.Catalogos.Area { Id = 6, Nombre = "Area 6", Activo = true },
                    new Domain.Catalogos.Area { Id = 7, Nombre = "Area 7", Activo = true },
                    new Domain.Catalogos.Area { Id = 8, Nombre = "Area 8", Activo = true },
                    new Domain.Catalogos.Area { Id = 9, Nombre = "Area 9", Activo = true }
                };
                await context.Areas.AddRangeAsync(areas);
            }

            // Puestos
            if (!await context.Puestos.AnyAsync())
            {
                var puestos = new List<Domain.Catalogos.Puesto>
                {
                    new() { Id = 4,   Nombre = "DIRECTOR",                       Nivel = "4",   TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 5,   Nombre = "SUBDIRECTOR",                    Nivel = "5",   TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 6,   Nombre = "GERENTE",                        Nivel = "6",   TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 7,   Nombre = "JEFE DE DEPARTAMENTO",           Nivel = "7",   TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 9,   Nombre = "AUX. ADMINISTRATIVO A",          Nivel = "9",   TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 10,  Nombre = "AUX. ADMINISTRATIVO B",          Nivel = "10",  TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 11,  Nombre = "AUX. ADMINISTRATIVO C",          Nivel = "11",  TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 12,  Nombre = "AUX. DE SERVICIOS GENERAL",      Nivel = "12",  TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 13,  Nombre = "MEDICO A",                       Nivel = "13",  TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 15,  Nombre = "ENFERMERO",                      Nivel = "15",  TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 19,  Nombre = "COMANDANTE A",                   Nivel = "19",  TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 22,  Nombre = "JEFE DE LOS CUERPOS AUXILIARES", Nivel = "22",  TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 25,  Nombre = "COORDINADOR",                    Nivel = "25",  TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 40,  Nombre = "SUBDIRECTOR A",                  Nivel = "40",  TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 50,  Nombre = "GERENTE A",                      Nivel = "50",  TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 61,  Nombre = "SECRETARIO TECNICO",             Nivel = "61",  TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 66,  Nombre = "AUX. ADMINISTRATIVO D",          Nivel = "66",  TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 67,  Nombre = "AUX. ADMINISTRATIVO E",          Nivel = "67",  TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 68,  Nombre = "SUBDIRECTOR OPERATIVO",          Nivel = "68",  TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 114, Nombre = "PENSIONADO",                     Nivel = "114", TipoEmpleo = "O", ClaveCategoria = "PEN", Activo = true },
                    new() { Id = 118, Nombre = "COMANDANTE B",                   Nivel = "118", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 119, Nombre = "COMANDANTE C",                   Nivel = "119", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 120, Nombre = "OFICIAL A",                      Nivel = "120", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 121, Nombre = "OFICIAL C",                      Nivel = "121", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 129, Nombre = "PENSIONADO FIDEICOMISO",         Nivel = "129", TipoEmpleo = "O", ClaveCategoria = "FID", Activo = true },
                    new() { Id = 141, Nombre = "GUARDIA C",                      Nivel = "141", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 150, Nombre = "MEDICO ESPECIALISTA A",          Nivel = "150", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 151, Nombre = "MEDICO ESPECIALISTA B",          Nivel = "151", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 152, Nombre = "MEDICO GENERAL A",               Nivel = "152", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 153, Nombre = "MEDICO GENERAL B",               Nivel = "153", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 155, Nombre = "SUPERVISOR DE ENFERMER",         Nivel = "155", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 157, Nombre = "ENFERMERO A",                    Nivel = "157", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 160, Nombre = "CAMILLERO",                      Nivel = "160", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 165, Nombre = "ASISTENTE DE DIRECCION",         Nivel = "165", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 177, Nombre = "JEFE DE FARMACIA B",             Nivel = "177", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 184, Nombre = "PARAMEDICO",                     Nivel = "184", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 185, Nombre = "JEFE DE TRABAJO SOCIAL",         Nivel = "185", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 186, Nombre = "TRABAJADOR SOCIAL",              Nivel = "186", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 187, Nombre = "JEFE DE MANTEMIENTO",            Nivel = "187", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 189, Nombre = "JEFE DE INTENDENCIA",            Nivel = "189", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 190, Nombre = "AUXILIAR INTENDENCIA",           Nivel = "190", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 191, Nombre = "TESORERO GENERAL Y CONTABILIDAD", Nivel = "191", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 194, Nombre = "ENLACE ADMINISTRATIVO A",        Nivel = "194", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 195, Nombre = "ENLACE ADMINISTRATIVO B",        Nivel = "195", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 196, Nombre = "ENLACE ADMINISTRATIVO C",        Nivel = "196", TipoEmpleo = "A", ClaveCategoria = "ADM", Activo = true },
                    new() { Id = 501, Nombre = "DIRECTOR OPERATIVO",             Nivel = "501", TipoEmpleo = "O", ClaveCategoria = "DTS", Activo = true },
                    new() { Id = 502, Nombre = "SUBDIRECTOR OPERATIVO",          Nivel = "502", TipoEmpleo = "O", ClaveCategoria = "DTS", Activo = true },
                    new() { Id = 511, Nombre = "COMANDANTE JEFE DE REGION",      Nivel = "511", TipoEmpleo = "O", ClaveCategoria = "JFS", Activo = true },
                    new() { Id = 513, Nombre = "COMANDANTE B",                   Nivel = "513", TipoEmpleo = "O", ClaveCategoria = "JFS", Activo = true },
                    new() { Id = 514, Nombre = "COMANDANTE C",                   Nivel = "514", TipoEmpleo = "O", ClaveCategoria = "JFS", Activo = true },
                    new() { Id = 515, Nombre = "CMDTE. JEFE DE SECTOR A",        Nivel = "515", TipoEmpleo = "O", ClaveCategoria = "JFS", Activo = true },
                    new() { Id = 516, Nombre = "CMDTE. JEFE DE SECTOR B",        Nivel = "516", TipoEmpleo = "O", ClaveCategoria = "JFS", Activo = true },
                    new() { Id = 517, Nombre = "CMDTE. JEFE DE SECTOR C",        Nivel = "517", TipoEmpleo = "O", ClaveCategoria = "JFS", Activo = true },
                    new() { Id = 521, Nombre = "OFICIAL A",                      Nivel = "521", TipoEmpleo = "O", ClaveCategoria = "ITS", Activo = true },
                    new() { Id = 522, Nombre = "OFICIAL B",                      Nivel = "522", TipoEmpleo = "O", ClaveCategoria = "ITS", Activo = true },
                    new() { Id = 523, Nombre = "OFICIAL C",                      Nivel = "523", TipoEmpleo = "O", ClaveCategoria = "ITS", Activo = true },
                    new() { Id = 524, Nombre = "GUARDIA A",                      Nivel = "524", TipoEmpleo = "O", ClaveCategoria = "ITS", Activo = true },
                    new() { Id = 525, Nombre = "GUARDIA B",                      Nivel = "525", TipoEmpleo = "O", ClaveCategoria = "ITS", Activo = true },
                    new() { Id = 526, Nombre = "GUARDIA C",                      Nivel = "526", TipoEmpleo = "O", ClaveCategoria = "ITS", Activo = true },
                    new() { Id = 531, Nombre = "OFICIAL A",                      Nivel = "531", TipoEmpleo = "O", ClaveCategoria = "ITA", Activo = true },
                    new() { Id = 532, Nombre = "OFICIAL B",                      Nivel = "532", TipoEmpleo = "O", ClaveCategoria = "ITA", Activo = true },
                    new() { Id = 533, Nombre = "OFICIAL C",                      Nivel = "533", TipoEmpleo = "O", ClaveCategoria = "ITA", Activo = true },
                    new() { Id = 534, Nombre = "GUARDIA A",                      Nivel = "534", TipoEmpleo = "O", ClaveCategoria = "ITA", Activo = true },
                    new() { Id = 535, Nombre = "GUARDIA B",                      Nivel = "535", TipoEmpleo = "O", ClaveCategoria = "ITA", Activo = true },
                    new() { Id = 536, Nombre = "GUARDIA C",                      Nivel = "536", TipoEmpleo = "O", ClaveCategoria = "ITA", Activo = true },
                    new() { Id = 541, Nombre = "OFICIAL A",                      Nivel = "541", TipoEmpleo = "O", ClaveCategoria = "CUA", Activo = true },
                    new() { Id = 542, Nombre = "OFICIAL B",                      Nivel = "542", TipoEmpleo = "O", ClaveCategoria = "CUA", Activo = true },
                    new() { Id = 543, Nombre = "OFICIAL C",                      Nivel = "543", TipoEmpleo = "O", ClaveCategoria = "CUA", Activo = true },
                    new() { Id = 544, Nombre = "GUARDIA A",                      Nivel = "544", TipoEmpleo = "O", ClaveCategoria = "CUA", Activo = true },
                    new() { Id = 545, Nombre = "GUARDIA B",                      Nivel = "545", TipoEmpleo = "O", ClaveCategoria = "CUA", Activo = true },
                    new() { Id = 546, Nombre = "GUARDIA C",                      Nivel = "546", TipoEmpleo = "O", ClaveCategoria = "CUA", Activo = true },
                    new() { Id = 551, Nombre = "OFICIAL A",                      Nivel = "551", TipoEmpleo = "O", ClaveCategoria = "ESC", Activo = true },
                    new() { Id = 552, Nombre = "OFICIAL B",                      Nivel = "552", TipoEmpleo = "O", ClaveCategoria = "ESC", Activo = true },
                    new() { Id = 553, Nombre = "OFICIAL C",                      Nivel = "553", TipoEmpleo = "O", ClaveCategoria = "ESC", Activo = true },
                    new() { Id = 554, Nombre = "GUARDIA A",                      Nivel = "554", TipoEmpleo = "O", ClaveCategoria = "ESC", Activo = true },
                    new() { Id = 555, Nombre = "GUARDIA B",                      Nivel = "555", TipoEmpleo = "O", ClaveCategoria = "ESC", Activo = true },
                    new() { Id = 556, Nombre = "GUARDIA C",                      Nivel = "556", TipoEmpleo = "O", ClaveCategoria = "ESC", Activo = true },
                    new() { Id = 557, Nombre = "ASPIRANTE",                      Nivel = "557", TipoEmpleo = "O", ClaveCategoria = "ITS", Activo = true }
                };
                await context.Puestos.AddRangeAsync(puestos);
            }

            // Servicios
            if (!await context.Servicios.AnyAsync())
            {
                var servicios = new List<Domain.Catalogos.Servicio>
                {
                    new Domain.Catalogos.Servicio { Id = 1, Nombre = "Servicio 1", Activo = true },
                    new Domain.Catalogos.Servicio { Id = 2, Nombre = "Servicio 2", Activo = true },
                    new Domain.Catalogos.Servicio { Id = 3, Nombre = "Servicio 3", Activo = true },
                    new Domain.Catalogos.Servicio { Id = 4, Nombre = "Servicio 4", Activo = true },
                    new Domain.Catalogos.Servicio { Id = 5, Nombre = "Servicio 5", Activo = true }
                };
                await context.Servicios.AddRangeAsync(servicios);
            }

            // Regiones
            if (!await context.Regiones.AnyAsync())
            {
                var regiones = new List<Domain.Catalogos.Region>
                {
                    new Domain.Catalogos.Region { Id = 1,  Nombre = "Región 1",  Activo = true },
                    new Domain.Catalogos.Region { Id = 2,  Nombre = "Región 2",  Activo = true },
                    new Domain.Catalogos.Region { Id = 3,  Nombre = "Región 3",  Activo = true },
                    new Domain.Catalogos.Region { Id = 4,  Nombre = "Región 4",  Activo = true },
                    new Domain.Catalogos.Region { Id = 5,  Nombre = "Región 5",  Activo = true },
                    new Domain.Catalogos.Region { Id = 6,  Nombre = "Región 6",  Activo = true },
                    new Domain.Catalogos.Region { Id = 7,  Nombre = "Región 7",  Activo = true },
                    new Domain.Catalogos.Region { Id = 8,  Nombre = "Región 8",  Activo = true },
                    new Domain.Catalogos.Region { Id = 9,  Nombre = "Región 9",  Activo = true },
                    new Domain.Catalogos.Region { Id = 10, Nombre = "Región 10", Activo = true },
                    new Domain.Catalogos.Region { Id = 11, Nombre = "Región 11", Activo = true },
                    new Domain.Catalogos.Region { Id = 12, Nombre = "Región 12", Activo = true },
                    new Domain.Catalogos.Region { Id = 13, Nombre = "Región 13", Activo = true },
                    new Domain.Catalogos.Region { Id = 14, Nombre = "Región 14", Activo = true },
                    new Domain.Catalogos.Region { Id = 15, Nombre = "Región 15", Activo = true },
                    new Domain.Catalogos.Region { Id = 16, Nombre = "Región 16", Activo = true },
                    new Domain.Catalogos.Region { Id = 17, Nombre = "Región 17", Activo = true },
                    new Domain.Catalogos.Region { Id = 18, Nombre = "Región 18", Activo = true },
                    new Domain.Catalogos.Region { Id = 19, Nombre = "Región 19", Activo = true },
                    new Domain.Catalogos.Region { Id = 20, Nombre = "Región 20", Activo = true },
                    new Domain.Catalogos.Region { Id = 21, Nombre = "Región 21", Activo = true },
                    new Domain.Catalogos.Region { Id = 22, Nombre = "Región 22", Activo = true },
                    new Domain.Catalogos.Region { Id = 23, Nombre = "Región 23", Activo = true },
                    new Domain.Catalogos.Region { Id = 24, Nombre = "Región 24", Activo = true },
                    new Domain.Catalogos.Region { Id = 25, Nombre = "Región 25", Activo = true },
                    new Domain.Catalogos.Region { Id = 26, Nombre = "Región 26", Activo = true },
                    new Domain.Catalogos.Region { Id = 27, Nombre = "Región 27", Activo = true },
                    new Domain.Catalogos.Region { Id = 28, Nombre = "Región 28", Activo = true },
                    new Domain.Catalogos.Region { Id = 29, Nombre = "Región 29", Activo = true },
                    new Domain.Catalogos.Region { Id = 30, Nombre = "Región 30", Activo = true },
                    new Domain.Catalogos.Region { Id = 31, Nombre = "Región 31", Activo = true },
                    new Domain.Catalogos.Region { Id = 32, Nombre = "Región 32", Activo = true },
                    new Domain.Catalogos.Region { Id = 33, Nombre = "Región 33", Activo = true },
                    new Domain.Catalogos.Region { Id = 34, Nombre = "Región 34", Activo = true },
                    new Domain.Catalogos.Region { Id = 35, Nombre = "Región 35", Activo = true },
                    new Domain.Catalogos.Region { Id = 36, Nombre = "Región 36", Activo = true },
                    new Domain.Catalogos.Region { Id = 37, Nombre = "Región 37", Activo = true },
                    new Domain.Catalogos.Region { Id = 38, Nombre = "Región 38", Activo = true },
                    new Domain.Catalogos.Region { Id = 39, Nombre = "Región 39", Activo = true },
                    new Domain.Catalogos.Region { Id = 40, Nombre = "Región 40", Activo = true },
                    new Domain.Catalogos.Region { Id = 41, Nombre = "Región 41", Activo = true },
                    new Domain.Catalogos.Region { Id = 42, Nombre = "Región 42", Activo = true },
                    new Domain.Catalogos.Region { Id = 43, Nombre = "Región 43", Activo = true },
                    new Domain.Catalogos.Region { Id = 44, Nombre = "Región 44", Activo = true },
                    new Domain.Catalogos.Region { Id = 45, Nombre = "Región 45", Activo = true },
                    new Domain.Catalogos.Region { Id = 46, Nombre = "Región 46", Activo = true },
                    new Domain.Catalogos.Region { Id = 47, Nombre = "Región 47", Activo = true },
                    new Domain.Catalogos.Region { Id = 48, Nombre = "Región 48", Activo = true },
                    new Domain.Catalogos.Region { Id = 49, Nombre = "Región 49", Activo = true },
                    new Domain.Catalogos.Region { Id = 50, Nombre = "Región 50", Activo = true },
                    new Domain.Catalogos.Region { Id = 51, Nombre = "Región 51", Activo = true },
                    new Domain.Catalogos.Region { Id = 52, Nombre = "Región 52", Activo = true },
                    new Domain.Catalogos.Region { Id = 53, Nombre = "Región 53", Activo = true },
                    new Domain.Catalogos.Region { Id = 54, Nombre = "Región 54", Activo = true },
                    new Domain.Catalogos.Region { Id = 55, Nombre = "Región 55", Activo = true },
                    new Domain.Catalogos.Region { Id = 56, Nombre = "Región 56", Activo = true },
                    new Domain.Catalogos.Region { Id = 57, Nombre = "Región 57", Activo = true },
                    new Domain.Catalogos.Region { Id = 58, Nombre = "Región 58", Activo = true },
                    new Domain.Catalogos.Region { Id = 59, Nombre = "Región 59", Activo = true },
                    new Domain.Catalogos.Region { Id = 60, Nombre = "Región 60", Activo = true },
                    new Domain.Catalogos.Region { Id = 61, Nombre = "Región 61", Activo = true },
                    new Domain.Catalogos.Region { Id = 62, Nombre = "Región 62", Activo = true },
                    new Domain.Catalogos.Region { Id = 63, Nombre = "Región 63", Activo = true },
                    new Domain.Catalogos.Region { Id = 64, Nombre = "Región 64", Activo = true },
                    new Domain.Catalogos.Region { Id = 65, Nombre = "Región 65", Activo = true },
                    new Domain.Catalogos.Region { Id = 66, Nombre = "Región 66", Activo = true },
                    new Domain.Catalogos.Region { Id = 67, Nombre = "Región 67", Activo = true },
                    new Domain.Catalogos.Region { Id = 68, Nombre = "Región 68", Activo = true },
                    new Domain.Catalogos.Region { Id = 69, Nombre = "Región 69", Activo = true },
                    new Domain.Catalogos.Region { Id = 70, Nombre = "Región 70", Activo = true },
                    new Domain.Catalogos.Region { Id = 71, Nombre = "Región 71", Activo = true },
                    new Domain.Catalogos.Region { Id = 72, Nombre = "Región 72", Activo = true },
                    new Domain.Catalogos.Region { Id = 73, Nombre = "Región 73", Activo = true },
                    new Domain.Catalogos.Region { Id = 74, Nombre = "Región 74", Activo = true },
                    new Domain.Catalogos.Region { Id = 75, Nombre = "Región 75", Activo = true },
                    new Domain.Catalogos.Region { Id = 76, Nombre = "Región 76", Activo = true },
                    new Domain.Catalogos.Region { Id = 77, Nombre = "Región 77", Activo = true },
                    new Domain.Catalogos.Region { Id = 78, Nombre = "Región 78", Activo = true },
                    new Domain.Catalogos.Region { Id = 79, Nombre = "Región 79", Activo = true },
                    new Domain.Catalogos.Region { Id = 80, Nombre = "Región 80", Activo = true }
                };
                await context.Regiones.AddRangeAsync(regiones);
            }
             

            await context.SaveChangesAsync();
        }
    }
}
