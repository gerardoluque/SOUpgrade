using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using API.Domain.Seguridad;
 
using API.Domain.Ubicacion;
using API.Domain.Catalogos;

namespace API.Persistence
{
    public class DbInitializer
    {
        public static async Task SeedData(AppDbContext context,
            UserManager<AppUserIdentity> userManager,
            RoleManager<AppIdentityRole> roleManager,
            IConfiguration configuration)
        {
            // Crear roles predeterminados
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new AppIdentityRole { Name = "Admin", Activo = true, Descripcion = "Rol de Administradores", Value = "admon", TipoRol = TipoRoles.AdministradorSistema });
            }

            if (!await roleManager.RoleExistsAsync("Elemento"))
            {
                await roleManager.CreateAsync(new AppIdentityRole { Name = "Elemento", Activo = true, Descripcion = "Rol de usuario", Value = "elemento", TipoRol = TipoRoles.Elemento });
            }

            if (!await roleManager.RoleExistsAsync("Medico"))
            {
                await roleManager.CreateAsync(new AppIdentityRole { Name = "Medico", Activo = true, Descripcion = "Rol de médico", Value = "medico", TipoRol = TipoRoles.Medico });
            }

            if (!await roleManager.RoleExistsAsync("Administrativo"))
            {
                await roleManager.CreateAsync(new AppIdentityRole { Name = "Administrativo", Activo = true, Descripcion = "Rol administrativo", Value = "administrativo", TipoRol = TipoRoles.Administrativo });
            }

            if (!await roleManager.RoleExistsAsync("Archivista clinico"))
            {
                await roleManager.CreateAsync(new AppIdentityRole { Name = "Archivista clinico", Activo = true, Descripcion = "Rol archivista clinico", Value = "archivista", TipoRol = TipoRoles.Administrativo });
            }

            var adminGrupoId = Guid.NewGuid().ToString();
            var medicosGrupoId = Guid.NewGuid().ToString();
            var elementosGrupoId = Guid.NewGuid().ToString();
            var administrativosGrupoId = Guid.NewGuid().ToString();
            var archivistasGrupoId = Guid.NewGuid().ToString();


            if (!await context.Grupos.AnyAsync()) 
            {
                var grupos = new List<Grupo>
                {
                    new Grupo
                    {
                        Id = adminGrupoId,
                        Nombre = "ADMIN",
                        Descr = "Grupo para Administradores del Sistema",
                        Activo = true,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow
                    },
                    new Grupo
                    {
                        Id = medicosGrupoId,
                        Nombre = "MEDICOS",
                        Descr = "Grupo de Usuarios tipo Medicos",
                        Activo = true,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow
                    },
                    new Grupo
                    {
                        Id = elementosGrupoId,
                        Nombre = "ELEMENTOS",
                        Descr = "Grupo de Usuarios tipo Elementos",
                        Activo = true,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow
                    },
                    new Grupo
                    {
                        Id = archivistasGrupoId,
                        Nombre = "ADMINISTRATIVOS",
                        Descr = "Grupo de Usuarios tipo Administrativos",
                        Activo = true,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow
                    }
                };
                await context.Grupos.AddRangeAsync(grupos);
                await context.SaveChangesAsync();
            }

            // Crear un usuario de ejemplo
            if (!userManager.Users.Any())
            {
                var user = new AppUserIdentity
                {
                    UserName = "admincusaem",
                    Email = "admin@example.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, "Password123!");
                await userManager.AddToRoleAsync(user, "Admin");

                var medicoUser = new AppUserIdentity
                {
                    UserName = "medicocusaem",
                    Email = "",
                    EmailConfirmed = true,
                    TwoFactorEnabled = false
                };
                await userManager.CreateAsync(medicoUser, "Password123!");
                await userManager.AddToRoleAsync(medicoUser, "Medico");

                var archivistaUser = new AppUserIdentity
                {
                    UserName = "archivista",
                    Email = "",
                    EmailConfirmed = true,
                    TwoFactorEnabled = false
                };  
                await userManager.CreateAsync(archivistaUser, "Password123!");
                await userManager.AddToRoleAsync(archivistaUser, "Archivista clinico");
            }

            string usuarioAdminId = Guid.NewGuid().ToString();
            string usuarioMedicoId = Guid.NewGuid().ToString();
            string usuarioArchivistaId = Guid.NewGuid().ToString();

            if (!context.Usuarios.Any())
            {
                var adminUser = await userManager.FindByNameAsync("admincusaem");
                var usuario = new Usuario
                {
                    Id = usuarioAdminId,
                    Nombre = "Administrador",
                    PrimerApellido = "CUSAEM",
                    TiempoInactividad = 15,
                    Activo = true,
                    EsUsuarioAD = false,
                    RolId = (await roleManager.FindByNameAsync("Admin")).Id,
                    GrupoId = adminGrupoId,
                    AppUserIdentityId = adminUser.Id,
                    FechaCreacion = DateTime.UtcNow,
                    FechaUltimaActualizacion = DateTime.UtcNow
                };
                await context.Usuarios.AddAsync(usuario);

                var medicoUser = await userManager.FindByNameAsync("medicocusaem");
                var usuarioMedico = new Usuario
                {
                    Id = usuarioMedicoId,
                    Nombre = "Medico",
                    PrimerApellido = "CUSAEM",
                    TiempoInactividad = 15,
                    Activo = true,
                    EsUsuarioAD = false,
                    RolId = (await roleManager.FindByNameAsync("Medico")).Id,
                    GrupoId = medicosGrupoId,
                    AppUserIdentityId = medicoUser.Id,
                    FechaCreacion = DateTime.UtcNow,
                    FechaUltimaActualizacion = DateTime.UtcNow
                };
                await context.Usuarios.AddAsync(usuarioMedico);

                var archivistaUser = await userManager.FindByNameAsync("archivista");
                var usuarioArchivista = new Usuario
                {
                    Id = usuarioArchivistaId,
                    Nombre = "Archivista",
                    PrimerApellido = "CUSAEM",
                    TiempoInactividad = 15,
                    Activo = true,
                    EsUsuarioAD = false,
                    RolId = (await roleManager.FindByNameAsync("Archivista clinico")).Id,
                    GrupoId = archivistasGrupoId,
                    AppUserIdentityId = archivistaUser.Id,
                    FechaCreacion = DateTime.UtcNow,
                    FechaUltimaActualizacion = DateTime.UtcNow
                };
                await context.Usuarios.AddAsync(usuarioArchivista);

                await context.SaveChangesAsync();
            }

            // Crear procesos predeterminados
            if (!context.Procesos.Any())
            {
                // FASE 1: Insertar procesos PADRES (sin ProcesoPadreId)
                var procesosPadres = new List<Proceso>
                {
                    new Proceso
                    {
                        Descr = "Gestion de Usuario",
                        Tipo = "A",
                        Icono = "groups",
                        Activo = true,
                        Ruta = null,
                        ProcesoPadreId = null,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = null,
                        SistemaId = 1
                    },
                    new Proceso
                    {
                        Descr = "Bitacora",
                        Tipo = "A",
                        Icono = "book",
                        Activo = true,
                        Ruta = null,
                        ProcesoPadreId = null,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = null,
                        SistemaId = 1
                    },
                    new Proceso
                    {
                        Descr = "Inventarios",
                        Tipo = "A",
                        Icono = "books",
                        Activo = false,
                        Ruta = null,
                        ProcesoPadreId = null,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = null,
                        SistemaId = 1
                    },
                    new Proceso
                    {
                        Descr = "Inventarios",
                        Tipo = "A",
                        Icono = "badge",
                        Activo = false,
                        Ruta = null,
                        ProcesoPadreId = null,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = null,
                        SistemaId = 1
                    },
                    new Proceso
                    {
                        Descr = "Inventarios",
                        Tipo = "A",
                        Icono = "book-bookmark",
                        Activo = false,
                        Ruta = null,
                        ProcesoPadreId = null,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = null,
                        SistemaId = 1
                    },
                    new Proceso
                    {
                        Descr = "Archivo Clinico",
                        Tipo = "A",
                        Icono = "badge",
                        Activo = true,
                        Ruta = null,
                        ProcesoPadreId = null,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = "[{\"Agregar\":false,\"Editar\":true,\"Borrar\":false}]",
                        SistemaId = 1
                    },
                    new Proceso
                    {
                        Descr = "Atenciones",
                        Tipo = "A",
                        Icono = "book",
                        Activo = true,
                        Ruta = null,
                        ProcesoPadreId = null,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = null,
                        SistemaId = 2
                    },
                    new Proceso
                    {
                        Descr = "Farmacia",
                        Tipo = "A",
                        Icono = "book",
                        Activo = true,
                        Ruta = null,
                        ProcesoPadreId = null,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = "[{\"Agregar\":false,\"Editar\":true,\"Borrar\":false}]",
                        SistemaId = 2
                    },
                    new Proceso
                    {
                        Descr = "Direccion",
                        Tipo = "A",
                        Icono = "archive",
                        Activo = true,
                        Ruta = null,
                        ProcesoPadreId = null,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = null,
                        SistemaId = 2
                    }
                };

                await context.Procesos.AddRangeAsync(procesosPadres);
                await context.SaveChangesAsync();

                // Obtener los IDs de los procesos padres recién insertados
                var gestionUsuario = await context.Procesos.FirstAsync(p => p.Descr == "Gestion de Usuario" && p.Tipo == "A");
                var bitacora = await context.Procesos.FirstAsync(p => p.Descr == "Bitacora" && p.Tipo == "A");
                var inventarios = await context.Procesos.FirstAsync(p => p.Descr == "Inventarios" && p.Tipo == "A" && p.Icono == "books");
                var archivoClinico = await context.Procesos.FirstAsync(p => p.Descr == "Archivo Clinico");
                var atenciones = await context.Procesos.FirstAsync(p => p.Descr == "Atenciones");
                var farmacia = await context.Procesos.FirstAsync(p => p.Descr == "Farmacia");
                var direccion = await context.Procesos.FirstAsync(p => p.Descr == "Direccion");

                // FASE 2: Insertar procesos HIJOS (con ProcesoPadreId válidos)
                var procesosHijos = new List<Proceso>
                {
                    new Proceso
                    {
                        Descr = "Roles",
                        Tipo = "P",
                        Icono = null,
                        Activo = true,
                        Ruta = "RolesList",
                        ProcesoPadreId = gestionUsuario.Id,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = "[{\"Agregar\":true,\"Editar\":true,\"Borrar\":false}]",
                        SistemaId = 1
                    },
                    new Proceso
                    {
                        Descr = "Usuarios",
                        Tipo = "P",
                        Icono = null,
                        Activo = true,
                        Ruta = "UsuarioList",
                        ProcesoPadreId = gestionUsuario.Id,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = "[{\"Agregar\":true,\"Editar\":true,\"Inactivar\":true,\"SeleccionarRol\":true,\"SeleccionarGrupo\":true,\"Agregar permisos\":true}]",
                        SistemaId = 1
                    },
                    new Proceso
                    {
                        Descr = "Grupos",
                        Tipo = "P",
                        Icono = null,
                        Activo = true,
                        Ruta = "GruposList",
                        ProcesoPadreId = gestionUsuario.Id,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = "[{\"Agregar\":true,\"Editar\":true,\"Borrar\":false}]",
                        SistemaId = 1
                    },
                    new Proceso
                    {
                        Descr = "Proceso",
                        Tipo = "P",
                        Icono = null,
                        Activo = true,
                        Ruta = "ProcesoList",
                        ProcesoPadreId = gestionUsuario.Id,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = "[{\"Agregar\":true,\"Editar\":true,\"Borrar\":false}]",
                        SistemaId = 1
                    },
                    new Proceso
                    {
                        Descr = "Bitacora de Acceso",
                        Tipo = "P",
                        Icono = null,
                        Activo = true,
                        Ruta = "BitacoraList",
                        ProcesoPadreId = bitacora.Id,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = "[{\"Consultar\":false,\"Exportar\":false}]",
                        SistemaId = 1
                    },
                    new Proceso
                    {
                        Descr = "Bitacora de Errores",
                        Tipo = "P",
                        Icono = null,
                        Activo = true,
                        Ruta = "BitacoraErrorList",
                        ProcesoPadreId = bitacora.Id,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = "[{\"Consultar\":false,\"Exportar\":false}]",
                        SistemaId = 1
                    },
                    new Proceso
                    {
                        Descr = "Orden de Compra",
                        Tipo = "P",
                        Icono = null,
                        Activo = true,
                        Ruta = "OrdenCompra",
                        ProcesoPadreId = inventarios.Id,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = "[{\"Agregar\":true,\"Editar\":true,\"Borrar\":false}]",
                        SistemaId = 1
                    },
                    new Proceso
                    {
                        Descr = "Envios",
                        Tipo = "P",
                        Icono = null,
                        Activo = true,
                        Ruta = "Envios",
                        ProcesoPadreId = inventarios.Id,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = "[{\"Agregar\":true,\"Editar\":true,\"Borrar\":false}]",
                        SistemaId = 1
                    },
                    new Proceso
                    {
                        Descr = "Registrar Paciente",
                        Tipo = "P",
                        Icono = null,
                        Activo = false,
                        Ruta = "PacienteList",
                        ProcesoPadreId = archivoClinico.Id,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = "[{\"Agregar\":false,\"Editar\":true,\"Borrar\":false}]",
                        SistemaId = 2
                    },
                    new Proceso
                    {
                        Descr = "Buscar Paciente",
                        Tipo = "P",
                        Icono = null,
                        Activo = false,
                        Ruta = "BuscarPaciente",
                        ProcesoPadreId = archivoClinico.Id,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = "[{\"Agregar\":false,\"Editar\":true,\"Borrar\":false}]",
                        SistemaId = 2
                    },
                    new Proceso
                    {
                        Descr = "Monitor Atenciones",
                        Tipo = "P",
                        Icono = "book",
                        Activo = true,
                        Ruta = "MonitorPacienteList",
                        ProcesoPadreId = archivoClinico.Id,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = "[{\"Agregar\":false,\"Editar\":true,\"Borrar\":false}]",
                        SistemaId = 2
                    },
                    new Proceso
                    {
                        Descr = "Reporte Diario",
                        Tipo = "P",
                        Icono = null,
                        Activo = false,
                        Ruta = "ReporteDiario",
                        ProcesoPadreId = archivoClinico.Id,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = "[{\"Agregar\":false,\"Editar\":true,\"Borrar\":false}]",
                        SistemaId = 2
                    },
                    new Proceso
                    {
                        Descr = "Consulta Atenciones",
                        Tipo = "P",
                        Icono = "archive",
                        Activo = false,
                        Ruta = "ConsultaAtenciones",
                        ProcesoPadreId = archivoClinico.Id,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = "[{\"Agregar\":false,\"Editar\":true,\"Borrar\":false}]",
                        SistemaId = 2
                    },
                    new Proceso
                    {
                        Descr = "Consulta de Medicos",
                        Tipo = "P",
                        Icono = "archive",
                        Activo = true,
                        Ruta = "ConsultaMedica",
                        ProcesoPadreId = archivoClinico.Id,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = "[{\"RegistrarMedico\":false,\"EditarMedico\":true}]",
                        SistemaId = 2
                    },
                    new Proceso
                    {
                        Descr = "Consulta de Paciente",
                        Tipo = "P",
                        Icono = "archive",
                        Activo = true,
                        Ruta = "ConsultaPaciente",
                        ProcesoPadreId = archivoClinico.Id,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = "[{\"Agregar\":false,\"Editar\":true,\"Borrar\":false}]",
                        SistemaId = 2
                    },
                    new Proceso
                    {
                        Descr = "Consulta Atencion",
                        Tipo = "P",
                        Icono = "watch",
                        Activo = true,
                        Ruta = "MonitorPacienteMedico",
                        ProcesoPadreId = atenciones.Id,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = "[{\"Agregar\":false,\"Editar\":true,\"Borrar\":false}]",
                        SistemaId = 2
                    },
                    new Proceso
                    {
                        Descr = "Registro Medicamento",
                        Tipo = "P",
                        Icono = "badge",
                        Activo = true,
                        Ruta = "RegistroMedicamentoList",
                        ProcesoPadreId = farmacia.Id,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = "[{\"Agregar\":false,\"Editar\":true,\"Borrar\":false}]",
                        SistemaId = 2
                    },
                    new Proceso
                    {
                        Descr = "Entrada Medicamento",
                        Tipo = "P",
                        Icono = "badge",
                        Activo = true,
                        Ruta = "EntradaMedicamentosList",
                        ProcesoPadreId = farmacia.Id,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = "[{\"Agregar\":false,\"Editar\":true,\"Borrar\":false}]",
                        SistemaId = 2
                    },
                    new Proceso
                    {
                        Descr = "Salida Medicamentos",
                        Tipo = "P",
                        Icono = "badge",
                        Activo = true,
                        Ruta = "SalidaMedicamentoList",
                        ProcesoPadreId = farmacia.Id,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = "[{\"Agregar\":false,\"Editar\":true,\"Borrar\":false}]",
                        SistemaId = 2
                    },
                    new Proceso
                    {
                        Descr = "Monitor",
                        Tipo = "P",
                        Icono = "badge",
                        Activo = true,
                        Ruta = "MonitorDireccion",
                        ProcesoPadreId = direccion.Id,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow,
                        Acciones = "[{\"Agregar\":false,\"Editar\":true,\"Borrar\":false}]",
                        SistemaId = 2
                    }
                };

                await context.Procesos.AddRangeAsync(procesosHijos);
                await context.SaveChangesAsync();
            }

            if (!context.RolesProcesos.Any())
            {
                var adminRole = await roleManager.FindByNameAsync("Admin");
                var allProcesos = await context.Procesos.ToListAsync();
                var rolesProcesos = allProcesos.Select(p => new RolProceso
                {
                    RolId = adminRole.Id,
                    ProcesoId = p.Id
                }).ToList();
                await context.RolesProcesos.AddRangeAsync(rolesProcesos);

                var medicoRole = await roleManager.FindByNameAsync("Medico");
                var procesoAtenciones = allProcesos.FirstOrDefault(p => p.Descr == "Atenciones");
                var medicoProcesos = allProcesos
                    .Where(p => p.SistemaId == 2 && p.ProcesoPadreId == procesoAtenciones?.Id || p.Id == procesoAtenciones?.Id)
                    .Select(p => new RolProceso
                    {
                        RolId = medicoRole.Id,
                        ProcesoId = p.Id
                    }).ToList();
                await context.RolesProcesos.AddRangeAsync(medicoProcesos);

                var archivistaRole = await roleManager.FindByNameAsync("Archivista clinico");
                var archivoClinico = allProcesos.FirstOrDefault(p => p.Descr == "Archivo Clinico");
                var archivistaProcesos = allProcesos
                    .Where(p => p.SistemaId == 2 && p.ProcesoPadreId == archivoClinico?.Id || p.Id == archivoClinico?.Id)
                    .Select(p => new RolProceso
                    {
                        RolId = archivistaRole.Id,
                        ProcesoId = p.Id
                    }).ToList();    
                await context.RolesProcesos.AddRangeAsync(archivistaProcesos);

                await context.SaveChangesAsync();
            }

            if (!context.Permisos.Any())
            {
                var allRolesProcesos = await context.RolesProcesos.ToListAsync();

                var procesoUsuarios = await context.Procesos
                    .Where(p => p.Ruta == "UsuarioList")
                    .FirstOrDefaultAsync();

                string accionUsuario = "[{\"Agregar\":true,\"Editar\":true,\"Inactivar\":true,\"SeleccionarRol\":true,\"SeleccionarGrupo\":true,\"Agregar permisos\":true}]";

                var permisos = allRolesProcesos.Select(rp => new Permiso
                {
                    UsuarioId = usuarioAdminId,
                    RolId = rp.RolId,
                    ProcesoId = rp.ProcesoId,
                    Acceso = procesoUsuarios != null && procesoUsuarios.Id == rp.ProcesoId 
                        ? accionUsuario 
                        : context.Procesos.First(p => p.Id == rp.ProcesoId).Acciones ?? string.Empty,
                }).ToList();
                await context.Permisos.AddRangeAsync(permisos);
                await context.SaveChangesAsync();
            }

            // Crear corporaciones predeterminadas
            if (!context.Corporaciones.Any())
            {
                var corporaciones = new List<Corporacion>
                {
                    new Corporacion
                    {
                        Id = Guid.NewGuid().ToString(),
                        Nombre = "Loma",
                        Descripcion = "Corporación la Loma",
                    },
                    new Corporacion
                    {
                        Id = Guid.NewGuid().ToString(),
                        Nombre = "Santa Rosa",
                        Descripcion = "Corporación Santa Rosa",
                    }
                };
                await context.Corporaciones.AddRangeAsync(corporaciones);
                await context.SaveChangesAsync();
            }

            if (!context.UsuarioCorporaciones.Any())
            {
                var corporaciones = await context.Corporaciones.ToListAsync();
                var usuarioCorporaciones = corporaciones.Select(c => new UsuarioCorporacion
                {
                    UsuarioId = usuarioAdminId,
                    CorporacionId = c.Id
                }).ToList();
                await context.UsuarioCorporaciones.AddRangeAsync(usuarioCorporaciones);
                await context.SaveChangesAsync();
            }

            if (!context.EntidadFederativas.Any())
            {
                var entidades = new List<EntidadFederativa>
                {
                    new EntidadFederativa { Id = 0, Nombre = "NO DISPONIBLE", Abreviacion = "" },
                    new EntidadFederativa { Id = 1, Nombre = "AGUASCALIENTES", Abreviacion = "AS" },
                    new EntidadFederativa { Id = 2, Nombre = "BAJA CALIFORNIA", Abreviacion = "BC" },
                    new EntidadFederativa { Id = 3, Nombre = "BAJA CALIFORNIA SUR", Abreviacion = "BS" },
                    new EntidadFederativa { Id = 4, Nombre = "CAMPECHE", Abreviacion = "CC" },
                    new EntidadFederativa { Id = 5, Nombre = "COAHUILA", Abreviacion = "CL" },
                    new EntidadFederativa { Id = 6, Nombre = "COLIMA", Abreviacion = "CM" },
                    new EntidadFederativa { Id = 7, Nombre = "CHIAPAS", Abreviacion = "CS" },
                    new EntidadFederativa { Id = 8, Nombre = "CHIHUAHUA", Abreviacion = "CH" },
                    new EntidadFederativa { Id = 9, Nombre = "CIUDAD DE MÉXICO", Abreviacion = "DF" },
                    new EntidadFederativa { Id = 10, Nombre = "DURANGO", Abreviacion = "DG" },
                    new EntidadFederativa { Id = 11, Nombre = "GUANAJUATO", Abreviacion = "GT" },
                    new EntidadFederativa { Id = 12, Nombre = "GUERRERO", Abreviacion = "GR" },
                    new EntidadFederativa { Id = 13, Nombre = "HIDALGO", Abreviacion = "HG" },
                    new EntidadFederativa { Id = 14, Nombre = "JALISCO", Abreviacion = "JC" },
                    new EntidadFederativa { Id = 15, Nombre = "MÉXICO", Abreviacion = "MC" },
                    new EntidadFederativa { Id = 16, Nombre = "MICHOACÁN", Abreviacion = "MN" },
                    new EntidadFederativa { Id = 17, Nombre = "MORELOS", Abreviacion = "MS" },
                    new EntidadFederativa { Id = 18, Nombre = "NAYARIT", Abreviacion = "NT" },
                    new EntidadFederativa { Id = 19, Nombre = "NUEVO LEÓN", Abreviacion = "NL" },
                    new EntidadFederativa { Id = 20, Nombre = "OAXACA", Abreviacion = "OC" },
                    new EntidadFederativa { Id = 21, Nombre = "PUEBLA", Abreviacion = "PL" },
                    new EntidadFederativa { Id = 22, Nombre = "QUERÉTARO", Abreviacion = "QT" },
                    new EntidadFederativa { Id = 23, Nombre = "QUINTANA ROO", Abreviacion = "QR" },
                    new EntidadFederativa { Id = 24, Nombre = "SAN LUIS POTOSÍ", Abreviacion = "SP" },
                    new EntidadFederativa { Id = 25, Nombre = "SINALOA", Abreviacion = "SL" },
                    new EntidadFederativa { Id = 26, Nombre = "SONORA", Abreviacion = "SR" },
                    new EntidadFederativa { Id = 27, Nombre = "TABASCO", Abreviacion = "TC" },
                    new EntidadFederativa { Id = 28, Nombre = "TAMAULIPAS", Abreviacion = "TS" },
                    new EntidadFederativa { Id = 29, Nombre = "TLAXCALA", Abreviacion = "TL" },
                    new EntidadFederativa { Id = 30, Nombre = "VERACRUZ", Abreviacion = "VZ" },
                    new EntidadFederativa { Id = 31, Nombre = "YUCATÁN", Abreviacion = "YN" },
                    new EntidadFederativa { Id = 32, Nombre = "ZACATECAS", Abreviacion = "ZS" },
                    new EntidadFederativa { Id = 33, Nombre = "EXTRANJERO", Abreviacion = "EX" }
                };
                await context.EntidadFederativas.AddRangeAsync(entidades);
                await context.SaveChangesAsync();
            }

            if (!context.Sistemas.Any())
            {
                var sistemas = new List<Sistema>
                {
                    new Sistema
                    {
                        Id = 1,
                        Nombre = "Sistema Administrativo",
                        Descripcion = "Sistema para la gestión administrativa",
                        Activo = true
                    },
                    new Sistema
                    {
                        Id = 2,
                        Nombre = "Sistema Clínico",
                        Descripcion = "Sistema para la gestión clínica",
                        Activo = true
                    },

                };
                await context.Sistemas.AddRangeAsync(sistemas);
                await context.SaveChangesAsync();
            }

            if (!context.BasesDatos.Any())
            {
                var serverName = configuration["DatabaseSettings:ServerName"] ?? "localhost";

                var basesDatos = new List<BaseDatos>
                {
                    new BaseDatos
                    {
                        Id = 1,
                        Nombre = "BaseDatosAdministrativa",
                        Descripcion = "Base de datos para el sistema administrativo",
                        DatabaseName = "BaseDatosAdministrativa",
                        ServerName = serverName,
                    },
                    new BaseDatos
                    {
                        Id = 2,
                        Nombre = "BaseDatosClinica",
                        Descripcion = "Base de datos para el sistema clínico",
                        DatabaseName = "Clinica",
                        ServerName = serverName,
                    }
                };
                await context.BasesDatos.AddRangeAsync(basesDatos);
                await context.SaveChangesAsync();
            }

            if (!context.CorporacionSistemaBDs.Any())
            {
                var coporacionSistemaBDs = new List<CorporacionSistemaBD>
                {
                     new CorporacionSistemaBD
                    {
                        CorporacionId = context.Corporaciones.FirstOrDefault(d=>d.Nombre == "Loma").Id,
                        SistemaId = 2,
                        BaseDatosId = 2
                    }
                };
                await context.CorporacionSistemaBDs.AddRangeAsync(coporacionSistemaBDs);
                await context.SaveChangesAsync();
            }

            // Seed Estados Civiles
            // Seed Estados Civiles
            var estadosCiviles = new List<EstadoCivil>
            {
                new EstadoCivil
                {
                    Id = 0,
                    Nombre = "Otro",
                    Descripcion = "Otro estado civil no especificado",
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow,
                    FechaUltimaActualizacion = DateTime.UtcNow
                },
                new EstadoCivil
                {
                    Id = 1,
                    Nombre = "Soltero",
                    Descripcion = "Persona soltera",
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow,
                    FechaUltimaActualizacion = DateTime.UtcNow
                },
                new EstadoCivil
                {
                    Id = 2,
                    Nombre = "Casado",
                    Descripcion = "Persona casada",
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow,
                    FechaUltimaActualizacion = DateTime.UtcNow
                },
                new EstadoCivil
                {
                    Id = 3,
                    Nombre = "Divorciado",
                    Descripcion = "Persona divorciada",
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow,
                    FechaUltimaActualizacion = DateTime.UtcNow
                },
                new EstadoCivil
                {
                    Id = 4,
                    Nombre = "Viudo",
                    Descripcion = "Persona viuda",
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow,
                    FechaUltimaActualizacion = DateTime.UtcNow
                },
                new EstadoCivil
                {
                    Id = 5,
                    Nombre = "Unión Libre",
                    Descripcion = "Persona en unión libre",
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow,
                    FechaUltimaActualizacion = DateTime.UtcNow
                },
                new EstadoCivil
                {
                    Id = 6,
                    Nombre = "Separado",
                    Descripcion = "Persona separada",
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow,
                    FechaUltimaActualizacion = DateTime.UtcNow
                },
                new EstadoCivil
                {
                    Id = 7,
                    Nombre = "Concubinato",
                    Descripcion = "Persona en concubinato",
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow,
                    FechaUltimaActualizacion = DateTime.UtcNow
                }
            };

            foreach (var estado in estadosCiviles)
            {
                if (!context.EstadosCiviles.Any(e => e.Nombre == estado.Nombre))
                {
                    await context.EstadosCiviles.AddAsync(estado);
                }
            }
            await context.SaveChangesAsync();

            // Seed Niveles Educativos
            if (!context.NivelesEducativos.Any())
            {
                var nivelesEducativos = new List<NivelEducativo>
                {
                    new NivelEducativo
                    {
                        Id = 0,
                        Nombre = "Otro",
                        Descripcion = "Otro nivel educativo no especificado",
                        Activo = true,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow
                    },
                    new NivelEducativo
                    {
                        Id = 1,
                        Nombre = "Primaria",
                        Descripcion = "Educación primaria",
                        Activo = true,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow
                    },
                    new NivelEducativo
                    {
                        Id = 2,
                        Nombre = "Secundaria",
                        Descripcion = "Educación secundaria",
                        Activo = true,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow
                    },
                    new NivelEducativo
                    {
                        Id = 3,
                        Nombre = "Preparatoria",
                        Descripcion = "Educación preparatoria o bachillerato",
                        Activo = true,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow
                    },
                    new NivelEducativo
                    {
                        Id = 4,
                        Nombre = "Licenciatura",
                        Descripcion = "Educación universitaria de licenciatura",
                        Activo = true,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow
                    },
                    new NivelEducativo
                    {
                        Id = 5,
                        Nombre = "Maestría",
                        Descripcion = "Posgrado de maestría",
                        Activo = true,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow
                    },
                    new NivelEducativo
                    {
                        Id = 6,
                        Nombre = "Doctorado",
                        Descripcion = "Posgrado de doctorado",
                        Activo = true,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaActualizacion = DateTime.UtcNow
                    }
                };
                await context.NivelesEducativos.AddRangeAsync(nivelesEducativos);
                await context.SaveChangesAsync();
            }
        }
    }
}