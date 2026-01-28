using MongoDB.Driver;
using TelegramLite.Domain.Entities;
using TelegramLite.Infrastructure.Data.MongoDB;

namespace TelegramLite.Infrastructure.Data.Repositories;

/// <summary>
/// Repository for Backup entities.
/// </summary>
public interface IBackupRepository : IRepository<Backup>
{
    /// <summary>
    /// Finds backups older than a specific date.
    /// </summary>
    /// <param name="olderThan">The date threshold.</param>
    /// <returns>A list of backups older than the specified date.</returns>
    Task<IEnumerable<Backup>> FindOlderThanAsync(DateTime olderThan);

    /// <summary>
    /// Finds the most recent successful backup.
    /// </summary>
    /// <returns>The most recent completed backup, or null if none found.</returns>
    Task<Backup?> FindLatestSuccessfulAsync();
}

/// <summary>
/// MongoDB implementation of IBackupRepository.
/// </summary>
public class BackupRepository : MongoRepository<Backup>, IBackupRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BackupRepository"/> class.
    /// </summary>
    /// <param name="context">The MongoDB context.</param>
    public BackupRepository(MongoDbContext context) : base(context.Backups)
    {
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Backup>> FindOlderThanAsync(DateTime olderThan)
    {
        return await FindAsync(b => b.CreatedAt < olderThan);
    }

    /// <inheritdoc/>
    public async Task<Backup?> FindLatestSuccessfulAsync()
    {
        var backups = await _collection
            .Find(b => b.Status == BackupStatus.Completed)
            .SortByDescending(b => b.CreatedAt)
            .Limit(1)
            .ToListAsync();

        return backups.FirstOrDefault();
    }
}
