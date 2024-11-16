using System.Security.Claims;

using Domain.DTOs;

namespace Domain.Interfaces.Application
{
    public interface IAuthService
    {
        Task Authentication(Request.Authentication request, IEnumerable<Claim> claims, CancellationToken token);
    }
}