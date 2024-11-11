using Domain.Interfaces.Application;

using Microsoft.EntityFrameworkCore;

namespace Domain.Interfaces.Infrastructure
{
    public interface IRepositoryUtilities<TLogger, TDBContext>
        where TLogger : class
        where TDBContext : DbContext
    {
        /// <summary>
        ///     Asynchronous method that wraps a database and cache operation in a <c>try-catch</c> 
        ///     block, providing exception handling, logging, and cancellation support. <br />
        ///     <br />
        ///     - The method accepts a <c>func</c> delegate that will be executed within the <c>try</c> block. <br />
        ///     - The delegate should only accept types that are safe for use in asynchronous operations. <br />
        ///     - <see cref="CancellationToken"/> and other dependencies such as <see cref="IDbContextFactory{TContext}"/> and 
        ///     <see cref="ICacheService"/> are mandatory for proper operation execution. <br />
        ///     - The method logs any exceptions that occur during the execution of <c>func</c> and rethrows them. <br />
        ///     <br /><br />
        ///     <c>Example usage:</c> <br /><br />
        ///     <code>
        ///         var result = await _utilities.TryWrapper(async (contextFactory, cache, ct) =>
        ///         {
        ///             await using var context = await contextFactory.CreateDbContextAsync(ct);
        ///             // Your logic with the database and cache
        ///             return someData;
        ///         }, cancellationToken);
        ///     </code>
        /// </summary>
        /// <remarks>
        ///     <c>IMPORTANT!</c><br />
        ///     When passing parameters to a closure, make sure you only pass parameters <br />
        ///     of type <see cref="ValueType"/>. Passing reference types may lead to large memory allocations <br />
        ///     and potential memory leaks due to the closure capturing references to those objects. <br />
        /// </remarks>
        /// <returns>
        ///     Returns the result of the operation as an object of type <c>TData</c>  <br />
        ///     if the operation completes successfully, or <c>null</c> if an error occurs. <br />
        ///     The <c>TData</c> type must inherit from <see cref="Entities.Entities.EntityBase"/>. <br />
        /// </returns>
        Task<TData?> TryWrapper<TData>(
            Func<IDbContextFactory<TDBContext>, ICacheService, CancellationToken, Task<TData?>> func,
            CancellationToken? token = default
        ) where TData : Entities.Entities.EntityBase;
    }
}