using System.Linq.Expressions;
using MongoDB.Driver;

namespace TelegramLite.Infrastructure.Data.Repositories;

/// <summary>
/// Base MongoDB repository implementation.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
public class MongoRepository<T> : IRepository<T> where T : class
{
    protected readonly IMongoCollection<T> _collection;

    /// <summary>
    /// Initializes a new instance of the <see cref="MongoRepository{T}"/> class.
    /// </summary>
    /// <param name="collection">The MongoDB collection.</param>
    public MongoRepository(IMongoCollection<T> collection)
    {
        _collection = collection;
    }

    /// <inheritdoc/>
    public virtual async Task<T?> GetByIdAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter)
    {
        return await _collection.Find(filter).ToListAsync();
    }

    /// <inheritdoc/>
    public virtual async Task<T?> FindOneAsync(Expression<Func<T, bool>> filter)
    {
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public virtual async Task<T> CreateAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    /// <inheritdoc/>
    public virtual async Task<bool> UpdateAsync(string id, T entity)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        var result = await _collection.ReplaceOneAsync(filter, entity);
        return result.ModifiedCount > 0;
    }

    /// <inheritdoc/>
    public virtual async Task<bool> DeleteAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        var result = await _collection.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }

    /// <inheritdoc/>
    public virtual async Task<long> CountAsync(Expression<Func<T, bool>>? filter = null)
    {
        if (filter == null)
        {
            return await _collection.CountDocumentsAsync(_ => true);
        }
        return await _collection.CountDocumentsAsync(filter);
    }
}
