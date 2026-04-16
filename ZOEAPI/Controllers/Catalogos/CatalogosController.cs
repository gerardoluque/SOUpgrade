using API.Application.Catalogos.Queries;
using API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Catalogos
{
    public class CatalogosController : BaseApiController
    {
        [HttpGet("estadosCiviles")]
        public async Task<IActionResult> GetEstadosCiviles()
        {
            return HandleResult(await Mediator.Send(new GetEstadosCivilesQuery()));
        }

        [HttpGet("nivelesEducativos")]
        public async Task<IActionResult> GetNivelesEducativos()
        {
            return HandleResult(await Mediator.Send(new GetNivelesEducativosQuery()));
        }

        [HttpGet("areas")]
        public async Task<IActionResult> GetAreas()
        {
            return HandleResult(await Mediator.Send(new GetAreasQuery()));
        }

        [HttpGet("regiones")]
        public async Task<IActionResult> GetRegiones()
        {
            return HandleResult(await Mediator.Send(new GetRegionesQuery()));
        }

        [HttpGet("servicios")]
        public async Task<IActionResult> GetServicios()
        {
            return HandleResult(await Mediator.Send(new GetServiciosQuery()));
        }

        [HttpGet("puestos")]
        public async Task<IActionResult> GetPuestos()
        {
            return HandleResult(await Mediator.Send(new GetPuestosQuery()));
        }

        [HttpGet("serviciosSalud")]
        public async Task<IActionResult> GetServiciosSalud()
        {
            return HandleResult(await Mediator.Send(new GetServiciosSaludQuery()));
        }

        [HttpGet("entidadesFederativas")]
        public async Task<IActionResult> GetEntidadesFederativas()
        {
            return HandleResult(await Mediator.Send(new GetEntidadesFederativasQuery()));
        }
    }
}