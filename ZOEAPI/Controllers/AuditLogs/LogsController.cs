using API.Application.Logs.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AuditLogs
{
    public class LogsController : BaseApiController
    {
        [HttpPost("filtered")]
        public async Task<ActionResult> GetFilteredLogs([FromBody] LogQueries.GetFilteredLogs.Query query)
        {
            return HandleResult(await Mediator.Send(query));
        }
    }
}
