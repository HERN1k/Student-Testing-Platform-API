using Domain.DTOs;
using Domain.Interfaces.Application;

using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;

        public AuthService(ILogger<AuthService> logger)
        {
            _logger = logger;
        }

        public async Task Authentication(Request.Authentication request)
        {

        }
    }
}