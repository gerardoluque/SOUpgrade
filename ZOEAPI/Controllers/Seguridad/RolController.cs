using System.Collections.Generic;
using System.Threading.Tasks;
using API.Application.Seguridad.Roles.Commands;
using API.Application.Seguridad.Roles.Queries;
using API.DTOs.Seguridad;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Seguridad
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<ApplicationRoleDto>>> GetRoles()
        {
            return HandleResult(await Mediator.Send(new GetRolList.Query()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationRoleDto>> GetRol(string id)
        {
            return HandleResult(await Mediator.Send(new GetRoleById.Query { Id = id }));
        }

        [HttpPost]
        public async Task<ActionResult> CreateRol([FromBody] CreateRol.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRol(string id, [FromBody] UpdateRol.Command command)
        {
            command.Id = id;
            return HandleResult(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRol(string id)
        {
            return HandleResult(await Mediator.Send(new DeleteRol.Command { Id = id }));
        }
    }
}