using Domain.DTOs;

using Helpers.Extensions;
using Helpers.Utilities;

using Microsoft.EntityFrameworkCore;

using static Domain.Entities.Entities;

namespace Infrastructure.Repositories
{
    public sealed partial class UsersRepository
    {
        public async Task<User?> GetUserFromIdAsync(Guid id, CancellationToken token)
        {
            User? cachedUser = null;

            string cacheKey = string.Concat(CacheKeys.EntityUserId, id.ToString());

            cachedUser = await _cache.GetCacheAsync<User>(cacheKey);

            token.ThrowIfCancellationRequested();

            cachedUser ??= await _utilities.TryWrapper(async (contextFactory, cache, ct) =>
            {
                await using var context = await contextFactory.CreateDbContextAsync(ct);

                var userEntity = await context.Users
                    .AsNoTracking()
                    .SingleOrDefaultAsync(e => e.Id == id, cancellationToken: ct);

                if (userEntity != null)
                {
                    await cache.SetCacheAsync(cacheKey, userEntity);
                }

                return userEntity;
            }, token);

            return cachedUser;
        }

        public async Task<User> AddOrUpdateUserAsync(DTO.AddOrUpdateUserInDB dto, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));
            ArgumentException.ThrowIfNullOrEmpty(dto.Id, nameof(dto.Id));
            ArgumentException.ThrowIfNullOrEmpty(dto.Mail, nameof(dto.Mail));

            token.ThrowIfCancellationRequested();

            return await _utilities.TryWrapper(async (contextFactory, cache, data, ct) =>
            {
                if (!Guid.TryParse(data.Id, out Guid id) || id == Guid.Empty)
                {
                    throw new ArgumentException("The unique user ID is not valid");
                }

                string cacheKey = string.Concat(CacheKeys.EntityUserId, data.Id);

                await using var context = await contextFactory.CreateDbContextAsync(ct);

                User? user = await context.Users
                    .Where(user => user.Id == id)
                    .SingleOrDefaultAsync(ct);

                if (user != null)
                {
                    user.DisplayName = data.DisplayName;
                    user.Name = data.Name;
                    user.Surname = data.Surname;

                    await context.SaveChangesAsync(ct);

                    await cache.SetCacheAsync(cacheKey, user);
                }
                else
                {
                    user = new()
                    {
                        Id = id,
                        DisplayName = data.DisplayName ?? string.Empty,
                        Name = data.Name,
                        Surname = data.Surname,
                        Mail = data.Mail,
                        IsStudent = data.Mail.IsStudent()
                    };

                    context.Users.Add(user);
                    await context.SaveChangesAsync(ct);

                    await cache.SetCacheAsync(cacheKey, user);
                }

                return user;
            }, dto, token) ?? throw new InvalidOperationException();
        }
    }
}