using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using API.Application.Core;
using API.Application.Core.Audit;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        private IMediator? _mediator;
        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetService<IMediator>()
                ?? throw new Exception("IMediator services is unavailable");

        private IAuditService? _auditService;
        protected IAuditService AuditService =>
            _auditService ??= HttpContext.RequestServices.GetService<IAuditService>()
                ?? throw new Exception("IAuditService services is unavailable");

        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result.IsSuccess && result.Value != null)
            {
                return Ok(result.Value);
            }

            if (!result.IsSuccess && result.Code == 404)
            {
                return NotFound(result.Error);
            }

            return BadRequest(result.Error);
        }
    }
}