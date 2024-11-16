using System.Security.Claims;

using Domain.DTOs;

using Helpers.Extensions;
using Helpers.Utilities;

namespace Application.Services
{
    public sealed partial class AuthService
    {
        public async Task Authentication(Request.Authentication request, IEnumerable<Claim> claims, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentNullException.ThrowIfNull(claims, nameof(claims));

            DTO.AddOrUpdateUserInDB data = new(
                Id: claims.ClaimOrDefault(JWTClaims.Id, request.Id),
                DisplayName: claims.ClaimOrDefault(JWTClaims.DisplayName, request.DisplayName),
                Name: claims.ClaimOrDefault(JWTClaims.Name, request.Name),
                Surname: claims.ClaimOrDefault(JWTClaims.Surname, request.Surname),
                Mail: claims.ClaimOrDefault(JWTClaims.Mail, request.Mail)
            );

            await _usersRepository.AddOrUpdateUserAsync(data, token);
        }
    }
}