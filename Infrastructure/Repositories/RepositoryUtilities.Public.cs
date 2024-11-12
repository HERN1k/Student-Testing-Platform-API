using System.Diagnostics;

using Domain.Interfaces.Application;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public sealed partial class RepositoryUtilities<TLogger, TDBContext>
    {
        public async Task<TEntity?> TryWrapper<TData, TEntity>(
            Func<IDbContextFactory<TDBContext>, ICacheService, TData, CancellationToken, Task<TEntity?>> func,
            TData data,
            CancellationToken? token = default
        )
            where TData : class
            where TEntity : Domain.Entities.Entities.EntityBase
        {
            token?.ThrowIfCancellationRequested();

            try
            {
                if (func != null)
                {
                    ArgumentNullException.ThrowIfNull(data, nameof(data));

                    return await func(_contextFactory, _cache, data, token ?? new CancellationToken());
                }
                else
                {
                    throw new ArgumentNullException(nameof(func));
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.ToString());
#endif
                _logger.LogError(ex, "An error occurred during operation");
                throw;
            }
        }

        public async Task<TEntity?> TryWrapper<TEntity>(
            Func<IDbContextFactory<TDBContext>, ICacheService, CancellationToken, Task<TEntity?>> func,
            CancellationToken? token = default
        )
            where TEntity : Domain.Entities.Entities.EntityBase
        {
            token?.ThrowIfCancellationRequested();

            try
            {
                if (func != null)
                {
                    return await func(_contextFactory, _cache, token ?? new CancellationToken());
                }
                else
                {
                    throw new ArgumentNullException(nameof(func));
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.ToString());
#endif
                _logger.LogError(ex, "An error occurred during operation");
                throw;
            }
        }

        public async Task TryWrapper<TData>(
            Func<IDbContextFactory<TDBContext>, ICacheService, TData, CancellationToken, Task> func,
            TData data,
            CancellationToken? token = default
        )
            where TData : class
        {
            token?.ThrowIfCancellationRequested();

            try
            {
                if (func != null)
                {
                    ArgumentNullException.ThrowIfNull(data, nameof(data));

                    await func(_contextFactory, _cache, data, token ?? new CancellationToken());
                }
                else
                {
                    throw new ArgumentNullException(nameof(func));
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.ToString());
#endif
                _logger.LogError(ex, "An error occurred during operation");
                throw;
            }
        }

        public async Task TryWrapper(
            Func<IDbContextFactory<TDBContext>, ICacheService, CancellationToken, Task> func,
            CancellationToken? token = default
        )
        {
            token?.ThrowIfCancellationRequested();

            try
            {
                if (func != null)
                {
                    await func(_contextFactory, _cache, token ?? new CancellationToken());
                }
                else
                {
                    throw new ArgumentNullException(nameof(func));
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.ToString());
#endif
                _logger.LogError(ex, "An error occurred during operation");
                throw;
            }
        }
    }
}