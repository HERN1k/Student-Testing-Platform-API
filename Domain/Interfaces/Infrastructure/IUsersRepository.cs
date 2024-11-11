using Domain.DTOs;

using Entity = Domain.Entities.Entities;

namespace Domain.Interfaces.Infrastructure
{
    public interface IUsersRepository
    {
        Task<Entity.User?> GetUserFromIdAsync(Guid id, CancellationToken token);

        Task AddOrUpdateUserAsync(DTO.AddOrUpdateUserInDB data, CancellationToken token);
    }
}