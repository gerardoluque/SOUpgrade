using API.Application.Core;
using API.Domain.Catalogos;
using API.Domain.Ubicacion;
using MediatR;

namespace API.Application.Catalogos.Queries
{
    public class GetEstadosCivilesQuery : IRequest<Result<List<EstadoCivil>>> { }

    public class GetEstadosCivilesQueryHandler(ICatalogoService catalogoService) : IRequestHandler<GetEstadosCivilesQuery, Result<List<EstadoCivil>>>
    {
        public async Task<Result<List<EstadoCivil>>> Handle(GetEstadosCivilesQuery request, CancellationToken cancellationToken)
        {
            var estadosCiviles = await catalogoService.GetEstadosCivilesAsync();
            return Result<List<EstadoCivil>>.Success(estadosCiviles);
        }
    }

    public class GetNivelesEducativosQuery : IRequest<Result<List<NivelEducativo>>> { }

    public class GetNivelesEducativosQueryHandler(ICatalogoService catalogoService) : IRequestHandler<GetNivelesEducativosQuery, Result<List<NivelEducativo>>>
    {
        public async Task<Result<List<NivelEducativo>>> Handle(GetNivelesEducativosQuery request, CancellationToken cancellationToken)
        {
            var nivelesEducativos = await catalogoService.GetNivelesEducativosAsync();
            return Result<List<NivelEducativo>>.Success(nivelesEducativos);
        }
    }

    public class GetAreasQuery : IRequest<Result<List<Area>>> { }

    public class GetAreasQueryHandler(ICatalogoService catalogoService) : IRequestHandler<GetAreasQuery, Result<List<Area>>>
    {
        public async Task<Result<List<Area>>> Handle(GetAreasQuery request, CancellationToken cancellationToken)
        {
            var areas = await catalogoService.GetAreasAsync();
            return Result<List<Area>>.Success(areas);
        }
    }

    public class GetRegionesQuery : IRequest<Result<List<Region>>> { }

    public class GetRegionesQueryHandler(ICatalogoService catalogoService) : IRequestHandler<GetRegionesQuery, Result<List<Region>>>
    {
        public async Task<Result<List<Region>>> Handle(GetRegionesQuery request, CancellationToken cancellationToken)
        {
            var regiones = await catalogoService.GetRegionesAsync();
            return Result<List<Region>>.Success(regiones);
        }
    }

    public class GetServiciosQuery : IRequest<Result<List<Servicio>>> { }

    public class GetServiciosQueryHandler(ICatalogoService catalogoService) : IRequestHandler<GetServiciosQuery, Result<List<Servicio>>>
    {
        public async Task<Result<List<Servicio>>> Handle(GetServiciosQuery request, CancellationToken cancellationToken)
        {
            var servicios = await catalogoService.GetServiciosAsync();
            return Result<List<Servicio>>.Success(servicios);
        }
    }

    public class GetPuestosQuery : IRequest<Result<List<Puesto>>> { }

    public class GetPuestosQueryHandler(ICatalogoService catalogoService) : IRequestHandler<GetPuestosQuery, Result<List<Puesto>>>
    {
        public async Task<Result<List<Puesto>>> Handle(GetPuestosQuery request, CancellationToken cancellationToken)
        {
            var puestos = await catalogoService.GetPuestosAsync();
            return Result<List<Puesto>>.Success(puestos);
        }
    }

    public class GetServiciosSaludQuery : IRequest<Result<List<ServicioSalud>>> { }

    public class GetServiciosSaludQueryHandler(ICatalogoService catalogoService) : IRequestHandler<GetServiciosSaludQuery, Result<List<ServicioSalud>>>
    {
        public async Task<Result<List<ServicioSalud>>> Handle(GetServiciosSaludQuery request, CancellationToken cancellationToken)
        {
            var serviciosSalud = await catalogoService.GetServicioSaludAsync();
            return Result<List<ServicioSalud>>.Success(serviciosSalud);
        }
    }

    public class GetEntidadesFederativasQuery : IRequest<Result<List<EntidadFederativa>>> { }

    public class GetEntidadesFederativasQueryHandler(ICatalogoService catalogoService) : IRequestHandler<GetEntidadesFederativasQuery, Result<List<EntidadFederativa>>>
    {
        public async Task<Result<List<EntidadFederativa>>> Handle(GetEntidadesFederativasQuery request, CancellationToken cancellationToken)
        {
            var entidades = await catalogoService.GetEntidadesFederativasAsync();
            return Result<List<EntidadFederativa>>.Success(entidades);
        }
    }
}