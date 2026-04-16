using API.Domain.Catalogos;

using API.Domain.Seguridad;
using API.Domain.Ubicacion;

using API.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace API.Application.Core
{
    public interface ICatalogoService
    {
        Task<List<Corporacion>> GetCorporacionesAsync();
      

        // Métodos para entidades RH
        Task<List<Area>> GetAreasAsync();
        Task<List<Region>> GetRegionesAsync();
        Task<List<Servicio>> GetServiciosAsync();
        Task<List<Puesto>> GetPuestosAsync();
        Task<List<ServicioSalud>> GetServicioSaludAsync();
        Task<List<EntidadFederativa>> GetEntidadesFederativasAsync();

        // Métodos para catálogos generales
        Task<List<EstadoCivil>> GetEstadosCivilesAsync();
        Task<List<NivelEducativo>> GetNivelesEducativosAsync();

        // Métodos para catálogos de medicamentos
       

        // Nuevos métodos optimizados que retornan diccionarios (Id, Nombre)
        

        // Método para invalidar caché
        void InvalidateCache(string cacheKey);
        void InvalidateMedicamentosCache();
    }

    public class CatalogoService : ICatalogoService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ICorporacionDbContextFactory _factory;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(6);

        public CatalogoService(AppDbContext context, IMemoryCache cache, ICorporacionDbContextFactory factory, IMapper mapper)
        {
            _context = context;
            _cache = cache;
            _factory = factory;
            _mapper = mapper;
        }

        public async Task<List<EntidadFederativa>> GetEntidadesFederativasAsync()
        {
            return await _cache.GetOrCreateAsync("EntidadesFederativas", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheDuration;
                return await _context.EntidadFederativas.AsNoTracking().ToListAsync();
            });
        }

        public async Task<List<Corporacion>> GetCorporacionesAsync()
        {
            return await _cache.GetOrCreateAsync("Corporaciones", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheDuration;
                return await _context.Corporaciones.AsNoTracking().ToListAsync();
            });
        }

      

        public async Task<List<Area>> GetAreasAsync()
        {
            return await _cache.GetOrCreateAsync("Areas", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheDuration;
                await using var db = await _factory.CreateAsync();
                return await db.Areas.AsNoTracking().ToListAsync();
            });
        }

        public async Task<List<Region>> GetRegionesAsync()
        {
            return await _cache.GetOrCreateAsync("Regiones", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheDuration;
                await using var db = await _factory.CreateAsync();
                return await db.Regiones.AsNoTracking().ToListAsync();
            });
        }

        public async Task<List<Servicio>> GetServiciosAsync()
        {
            return await _cache.GetOrCreateAsync("Servicios", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheDuration;
                await using var db = await _factory.CreateAsync();
                return await db.Servicios.AsNoTracking().ToListAsync();
            });
        }

        public async Task<List<Puesto>> GetPuestosAsync()
        {
            return await _cache.GetOrCreateAsync("Puestos", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheDuration;
                await using var db = await _factory.CreateAsync();
                return await db.Set<Puesto>().AsNoTracking().ToListAsync();
            });
        }

        public async Task<List<ServicioSalud>> GetServicioSaludAsync()
        {
            return await _cache.GetOrCreateAsync("ServiciosSalud", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheDuration;
                await using var db = await _factory.CreateAsync();
                return await db.ServiciosSalud.AsNoTracking().ToListAsync();
            });
        }

        public async Task<List<EstadoCivil>> GetEstadosCivilesAsync()
        {
            return await _cache.GetOrCreateAsync("EstadosCiviles", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheDuration;
                return await _context.EstadosCiviles.AsNoTracking().Where(e => e.Activo).ToListAsync();
            }) ?? new List<EstadoCivil>();
        }

        public async Task<List<NivelEducativo>> GetNivelesEducativosAsync()
        {
            return await _cache.GetOrCreateAsync("NivelesEducativos", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheDuration;
                return await _context.NivelesEducativos.AsNoTracking().Where(n => n.Activo).ToListAsync();
            }) ?? new List<NivelEducativo>();
        }

       

        // ==================== NUEVOS MÉTODOS OPTIMIZADOS PARA DICCIONARIOS ====================

      

       

        

      
        public void InvalidateCache(string cacheKey)
        {
            _cache.Remove(cacheKey);
        }

        public void InvalidateMedicamentosCache()
        {
            _cache.Remove("Farmacias");
            _cache.Remove("Farmacias_Dictionary");
            _cache.Remove("MedicamentoTipoDocumentos");
            _cache.Remove("MedicamentoTipoArticulos");
            _cache.Remove("MedicamentoTipoArticulos_Dictionary");
            _cache.Remove("MedicamentoClasificaciones");
            _cache.Remove("MedicamentoClasificaciones_Dictionary");
            _cache.Remove("MedicamentoPresentaciones");
            _cache.Remove("MedicamentoPresentaciones_Dictionary");
            _cache.Remove("MedicamentoUnidadMedidas");
            _cache.Remove("MedicamentoUnidadMedidas_Dictionary");
            _cache.Remove("MedicamentoTipoControles");
            _cache.Remove("MedicamentoTipoControles_Dictionary");
        }
    }
}