﻿using Domain.DTOs;
using Domain.Interfaces.Application;

using Microsoft.AspNetCore.Mvc;

namespace TestingPlatformAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public sealed class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authentication;

        public AuthenticationController(IAuthService authentication)
        {
            _authentication = authentication;
        }

        [HttpPost("authentication")]
        public async Task<IActionResult> Authentication(Request.Authentication request, CancellationToken token)
        {
            await _authentication.Authentication(request, HttpContext.User.Claims, token);

            return Ok(new Response.Success());
        }
    }
}