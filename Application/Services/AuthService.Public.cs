using Domain.DTOs;

namespace Application.Services
{
    public sealed partial class AuthService
    {
        public async Task Authentication(Request.Authentication request, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            await _usersRepository.AddOrUpdateUserAsync(new(
                Id: request.Id,
                DisplayName: request.DisplayName,
                Name: request.Name,
                Surname: request.Surname,
                Mail: request.Mail
            ), token);
        }
    }
}