using Domain.DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestingPlatformAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public sealed class ApiController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new Response.Success());

        [Authorize]
        [HttpGet("uptime")]
        public IActionResult Uptime()
        {
            var response = new DTO.TimeResponse(DateTime.UtcNow - Program.StartupTime);

            return Ok(new Response.Time(response));
        }

        [Authorize(Policy = "Teacher")]
        [HttpGet("time")]
        public IActionResult Time(bool utc)
        {
            DTO.TimeResponse response;
            if (utc)
            {
                response = new(DateTime.UtcNow);
            }
            else
            {
                response = new(DateTime.Now);
            }

            return Ok(new Response.Time(response));
        }
    }
}