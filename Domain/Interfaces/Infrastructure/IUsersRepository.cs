using Domain.DTOs;

using Entity = Domain.Entities.Entities;

namespace Domain.Interfaces.Infrastructure
{
    public interface IUsersRepository
    {
        Task<Entity.User?> GetUserFromIdAsync(Guid id, CancellationToken token);

        Task<Entity.User> AddOrUpdateUserAsync(DTO.AddOrUpdateUserInDB dto, CancellationToken token);
    }
}