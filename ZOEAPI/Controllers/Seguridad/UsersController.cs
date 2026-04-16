using API.Application.Seguridad.Usuarios.Commands;
using API.Application.Seguridad.Usuarios.Queries;
using API.Domain.Seguridad;
using API.DTOs.Seguridad;
using API.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.Seguridad
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BaseApiController
    {
        private readonly UsuarioCacheService _usuarioCacheService;

        public UsersController(UsuarioCacheService usuarioCacheService) : base()
        {
            _usuarioCacheService = usuarioCacheService;
        }

        [HttpGet]
        [AuthorizeByTipoRol(TipoRoles.AdministradorSistema)]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            return HandleResult(await Mediator.Send(new GetUserList.Query()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(string id)
        {
            return HandleResult(await Mediator.Send(new GetUserById.Query { Id = id }));
        }

        [HttpPost]
        [AuthorizeByTipoRol(TipoRoles.AdministradorSistema)]
        public async Task<ActionResult<string>> CreateUser([FromBody] CreateUser.Command command)
        {
            _usuarioCacheService.ClearCache();
            return HandleResult(await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        [AuthorizeByTipoRol(TipoRoles.AdministradorSistema)]
        public async Task<ActionResult> UpdateUser(string id, [FromBody] UpdateUser.Command command)
        {
            command.Id = id;
            return HandleResult(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        [AuthorizeByTipoRol(TipoRoles.AdministradorSistema)]
        public async Task<ActionResult> DeleteUser(string id)
        {
            return HandleResult(await Mediator.Send(new ActiveInactiveUser.Command { Id = id, Activo = false }));
        }

        [HttpPost("clear-cache")]
        public IActionResult ClearCache()
        {
            _usuarioCacheService.ClearCache();
            return Ok("Cache de usuarios limpiado.");
        }
    }
}
