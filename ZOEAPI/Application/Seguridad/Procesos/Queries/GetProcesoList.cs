using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Domain;
using API.DTOs.Seguridad;
using API.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Seguridad.Procesos.Queries
{
    public class GetProcesoList
    {
        public class Query : IRequest<List<ProcesoDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<ProcesoDto>>
        {
            private readonly AppDbContext _context;

            public Handler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<List<ProcesoDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Procesos
                    .Include(p => p.Subprocesos) // Load child processes
                    .Select(p => new ProcesoDto
                    {
                        Id = p.Id,
                        Descr = p.Descr,
                        Tipo = p.Tipo,
                        Activo = p.Activo,
                        Ruta = p.Ruta,
                        Icono = p.Icono,
                        ProcesoPadreId = p.ProcesoPadreId,
                        SistemaId = p.SistemaId,
                        Subprocesos = p.Subprocesos.Select(sp => new ProcesoDto
                        {
                            Id = sp.Id,
                            Descr = sp.Descr,
                            Tipo = sp.Tipo,
                            Activo = sp.Activo
                        }).ToList()
                    })
                    .ToListAsync(cancellationToken);
            }
        }
    }
}