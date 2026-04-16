using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Domain;
using API.Persistence;
using API.Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using API.DTOs.Seguridad;

namespace API.Application.Seguridad.Corporaciones.Queries
{
    public class GetCorporacionList
    {
        public class Query : IRequest<Result<List<CorporacionDTO>>>
        {
        }

        public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Query, Result<List<CorporacionDTO>>>
        {
            public async Task<Result<List<CorporacionDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var corporaciones = await context.Corporaciones.ToListAsync(cancellationToken);
                return Result<List<CorporacionDTO>>.Success(mapper.Map<List<CorporacionDTO>>(corporaciones));
            }
        }
    }
}