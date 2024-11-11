using Domain.Interfaces.Application;
using Domain.Interfaces.Infrastructure;

using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public sealed partial class AuthService : IAuthService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUsersRepository usersRepository, ILogger<AuthService> logger)
        {
            ValidateConstructorArguments(usersRepository, logger);
            _usersRepository = usersRepository;
            _logger = logger;
        }

        private static void ValidateConstructorArguments(IUsersRepository usersRepository, ILogger<AuthService> logger)
        {
            ArgumentNullException.ThrowIfNull(usersRepository, nameof(usersRepository));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        }
    }
}