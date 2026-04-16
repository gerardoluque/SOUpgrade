using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using API.Application.Core.Audit;
using API.Infrastructure.Audit;
using API.DTOs.Seguridad;

namespace API.Controllers.Seguridad
{
    public class AuthAzureController : BaseApiController
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            var userIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Convert.ToString(HttpContext.Request.Headers["User-Agent"]);

            await AuditService.LogAsync(Enumerations.Audits.AuditEventType.Login, loginDto.Userid ?? string.Empty,
                loginDto.Username ?? "System", Enumerations.Audits.AuditEventType.Login.ToString(), userIp, userAgent);

            return Ok(new { message = "Login successful" });
        }

        [HttpGet("user-info")]
        public async Task<ActionResult> GetUserInfo()
        {
            if (User.Identity?.IsAuthenticated == false)
                return NoContent();

            return Ok(new
            {
                User.Identity.Name,
                Claims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            if (User.Identity?.IsAuthenticated == false)
                return NoContent();

            var userId = User
                .Claims
                .FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value ?? "System";

            var userIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Convert.ToString(HttpContext.Request.Headers["User-Agent"]);

            await AuditService.LogAsync(Enumerations.Audits.AuditEventType.Logout, userId, 
                User.Identity?.Name ?? "System", Enumerations.Audits.AuditEventType.Logout.ToString(), userIp, userAgent);

            return Ok(new { message = "Logout successful" });
        }
    }
}
