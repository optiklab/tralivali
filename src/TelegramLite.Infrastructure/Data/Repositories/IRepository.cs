using System.Linq.Expressions;

namespace TelegramLite.Infrastructure.Data.Repositories;

/// <summary>
/// Generic repository interface for CRUD operations.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Gets an entity by its ID.
    /// </summary>
    /// <param name="id">The entity ID.</param>
    /// <returns>The entity if found, otherwise null.</returns>
    Task<T?> GetByIdAsync(string id);

    /// <summary>
    /// Gets all entities.
    /// </summary>
    /// <returns>A list of all entities.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Finds entities matching a filter.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <returns>A list of matching entities.</returns>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter);

    /// <summary>
    /// Finds a single entity matching a filter.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <returns>The first matching entity, or null.</returns>
    Task<T?> FindOneAsync(Expression<Func<T, bool>> filter);

    /// <summary>
    /// Creates a new entity.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    /// <returns>The created entity.</returns>
    Task<T> CreateAsync(T entity);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="id">The entity ID.</param>
    /// <param name="entity">The entity with updated values.</param>
    /// <returns>True if updated successfully, otherwise false.</returns>
    Task<bool> UpdateAsync(string id, T entity);

    /// <summary>
    /// Deletes an entity by its ID.
    /// </summary>
    /// <param name="id">The entity ID.</param>
    /// <returns>True if deleted successfully, otherwise false.</returns>
    Task<bool> DeleteAsync(string id);

    /// <summary>
    /// Counts entities matching a filter.
    /// </summary>
    /// <param name="filter">The filter expression. If null, counts all entities.</param>
    /// <returns>The count of matching entities.</returns>
    Task<long> CountAsync(Expression<Func<T, bool>>? filter = null);
}
