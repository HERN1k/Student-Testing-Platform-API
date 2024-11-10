using Domain.DTOs;
using Domain.Interfaces.Application;

using Microsoft.AspNetCore.Mvc;

namespace TestingPlatformAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authentication;

        public AuthenticationController(IAuthService authentication)
        {
            _authentication = authentication;
        }

        [HttpPost("authentication")]
        public async Task<IActionResult> Authentication(Request.Authentication request)
        {
            await _authentication.Authentication(request);

            return Ok(new Response.Success());
        }
    }
}