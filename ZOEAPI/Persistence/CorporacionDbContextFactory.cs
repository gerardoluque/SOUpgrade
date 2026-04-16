using API.Domain.Seguridad;
using API.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Caching.Memory;

namespace API.Persistence
{
    public class CorporacionDbContextDesignFactory : IDesignTimeDbContextFactory<CorporacionDbContext>
    {
        public CorporacionDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CorporacionDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-H8H45TE\\SQLEXPRESS;Database=Clinica;Trusted_Connection=True;TrustServerCertificate=True;");
            return new CorporacionDbContext(optionsBuilder.Options);
        }
    }

    public class CorporacionDbContextFactory : ICorporacionDbContextFactory
    {
        private readonly AppDbContext _appDbContext;
        private readonly ICorporacionContextAccessor _accessor;
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CorporacionDbContextFactory(
            AppDbContext appDbContext,
            ICorporacionContextAccessor accessor,
            IMemoryCache cache,
            IHttpContextAccessor httpContextAccessor)
        {
            _appDbContext = appDbContext;
            _accessor = accessor;
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CorporacionDbContext> CreateAsync()
        {
            var corporacionId = _accessor.CorporacionId ?? throw new Exception("CorporacionId no proporcionado.");
            var sistemaId = _accessor.SistemaId ?? throw new Exception("SistemaId no proporcionado.");

            if (!short.TryParse(sistemaId, out _))
            {
                throw new Exception("SistemaId debe ser un valor numérico válido.");
            }

            var corporacion = await _cache.GetOrCreateAsync($"corp:{corporacionId} sistema:{sistemaId}", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);

                var corpSistemaBD = await _appDbContext
                .CorporacionSistemaBDs
                .Include(c => c.BaseDatos)
                .FirstOrDefaultAsync(c => c.CorporacionId == corporacionId &&
                    c.SistemaId == Convert.ToInt16(sistemaId)) ?? throw new Exception("Corporación no encontrada.");

                var userId = AuthContext.GetUserId(_httpContextAccessor.HttpContext?.User) ??
                    throw new Exception("UserId no esta presente en el token.");

                return corpSistemaBD;
            });

            var optionsBuilder = new DbContextOptionsBuilder<CorporacionDbContext>();
            optionsBuilder.UseSqlServer(BuildConnectionString(corporacion));

            var dbContext = new CorporacionDbContext(optionsBuilder.Options, _httpContextAccessor, _accessor);
            await dbContext.Database.MigrateAsync(); // aplica migraciones

            // Seed datos base de la BD de corporación
            await CorporacionDbInitializer.SeedData(dbContext, _appDbContext, corporacionId);

            return dbContext;
        }

        private string BuildConnectionString(CorporacionSistemaBD corporacion)
        {
            var connString = corporacion.BaseDatos?.ConnectionString 
                ?? throw new Exception("La plantilla de cadena de conexión no está definida.");

            return connString;
        }
    }
}
