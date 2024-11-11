using Domain.DTOs;

namespace Domain.Interfaces.Application
{
    public interface IAuthService
    {
        Task Authentication(Request.Authentication request, CancellationToken token);
    }
}