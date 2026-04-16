using API.Domain.Anexos;
using API.Domain.Audit;
using API.Domain.Catalogos;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using static API.Infrastructure.Audit.Enumerations.Audits;
using API.Infrastructure;
using API.Application.Core.Extensions; // <-- ajusta el namespace real de ICorporacionContextAccessor

namespace API.Persistence
{
    public class CorporacionDbContext : DbContext
    {
        private readonly IHttpContextAccessor? _httpContextAccessor;
        private readonly ICorporacionContextAccessor? _accessor; // <-- nuevo campo

      
        public DbSet<Area> Areas { get; set; }
        public DbSet<Region> Regiones { get; set; }
        public DbSet<Anexo> Anexos { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Puesto> Puestos { get; set; }
        public DbSet<ServicioSalud> ServiciosSalud { get; set; }
        
        public DbSet<EntityChangeLog> EntityChangeLogs { get; set; }
         
        
        

        // Constructor usado por EF / migraciones (sin HttpContext)
        public CorporacionDbContext(DbContextOptions<CorporacionDbContext> options)
            : base(options)
        {
        }

        // Constructor para inyección normal con auditoría
        public CorporacionDbContext(
            DbContextOptions<CorporacionDbContext> options,
            IHttpContextAccessor httpContextAccessor,
            ICorporacionContextAccessor accessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _accessor = accessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer("Server=J6CFQAT\\SQLEXPRESS;Database=Clinica;Trusted_Connection=True;TrustServerCertificate=True;")
                    .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
            }
            else
            {
                // Ensure the warning is ignored even when options are provided externally
                optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Id no autogenerado
            modelBuilder.Entity<Area>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<Region>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<Servicio>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<ServicioSalud>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<Puesto>().Property(e => e.Id).ValueGeneratedNever();

            // AtencionMedica
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
             

            var auditEntries = OnBeforeSaveChanges();
            var result = await base.SaveChangesAsync(cancellationToken);

            if (result == 0 || auditEntries.Count == 0)
                return result;

            EntityChangeLogs.AddRange(auditEntries);
            await base.SaveChangesAsync(cancellationToken);
            return result;
        }

        public override int SaveChanges()
        {
            

            var auditEntries = OnBeforeSaveChanges();
            var result = base.SaveChanges();

            if (result == 0 || auditEntries.Count == 0)
                return result;

            EntityChangeLogs.AddRange(auditEntries);
            base.SaveChanges();
            return result;
        }

        private List<EntityChangeLog> OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();

            // Asignar CorporacionTenantId antes de calcular auditoría
            SetCorporacionTenantIdIfNeeded();

            var userId = API.Infrastructure.AuthContext.GetUserId(_httpContextAccessor?.HttpContext?.User) ?? "System";
            var userName = API.Infrastructure.AuthContext.GetUserName(_httpContextAccessor?.HttpContext?.User) ?? "System";

            var auditEntries = new List<EntityChangeLog>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is EntityChangeLog ||
                    entry.Entity is not IAuditable ||
                    entry.State == EntityState.Detached ||
                    entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                var entityName = entry.Entity.GetType().Name;
                var entityId = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue?.ToString() ?? "";

                var changeType = entry.State switch
                {
                    EntityState.Added => EntityActionType.Created,
                    EntityState.Modified => EntityActionType.Updated,
                    EntityState.Deleted => EntityActionType.Deleted,
                    _ => EntityActionType.None
                };

                var changes = new Dictionary<string, object?>();

                foreach (var prop in entry.Properties)
                {
                    if (prop.IsTemporary ||
                        prop.Metadata.IsPrimaryKey() ||
                        prop.Metadata.Name.Equals("FechaUltimaActualizacion", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            changes[prop.Metadata.Name] = new { New = prop.CurrentValue };
                            break;
                        case EntityState.Deleted:
                            changes[prop.Metadata.Name] = new { Old = prop.OriginalValue };
                            break;
                        case EntityState.Modified when prop.IsModified:
                            changes[prop.Metadata.Name] = new { Old = prop.OriginalValue, New = prop.CurrentValue };
                            break;
                    }
                }

                if (changes.Count == 0)
                    continue;

                auditEntries.Add(new EntityChangeLog
                {
                    UserId = userId,
                    UserName = userName,
                    ChangeType = changeType,
                    EntityName = entityName,
                    EntityId = entityId,
                    Changes = JsonConvert.SerializeObject(changes)
                });
            }

            return auditEntries;
        }

        private void SetCorporacionTenantIdIfNeeded()
        {
            if (_accessor == null)
            {
                return;
            }

            var tenantId = _accessor.CorporacionId;
            if (tenantId.IsNullOrWhiteSpace())
            {
                return;
            }

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State != EntityState.Added && entry.State != EntityState.Modified)
                {
                    continue;
                }

                var entity = entry.Entity;

                 
            }
        }

         
    }
}
