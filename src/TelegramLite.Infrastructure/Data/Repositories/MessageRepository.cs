using MongoDB.Driver;
using TelegramLite.Domain.Entities;
using TelegramLite.Infrastructure.Data.MongoDB;

namespace TelegramLite.Infrastructure.Data.Repositories;

/// <summary>
/// Repository for Message entities.
/// </summary>
public interface IMessageRepository : IRepository<Message>
{
    /// <summary>
    /// Finds messages in a specific conversation.
    /// </summary>
    /// <param name="conversationId">The conversation ID.</param>
    /// <param name="limit">The maximum number of messages to return.</param>
    /// <param name="before">Optional timestamp to get messages before.</param>
    /// <returns>A list of messages ordered by creation time descending.</returns>
    Task<IEnumerable<Message>> FindByConversationIdAsync(string conversationId, int limit = 50, DateTime? before = null);

    /// <summary>
    /// Finds messages older than a specific date.
    /// </summary>
    /// <param name="olderThan">The date threshold.</param>
    /// <returns>A list of messages older than the specified date.</returns>
    Task<IEnumerable<Message>> FindOlderThanAsync(DateTime olderThan);
}

/// <summary>
/// MongoDB implementation of IMessageRepository.
/// </summary>
public class MessageRepository : MongoRepository<Message>, IMessageRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageRepository"/> class.
    /// </summary>
    /// <param name="context">The MongoDB context.</param>
    public MessageRepository(MongoDbContext context) : base(context.Messages)
    {
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Message>> FindByConversationIdAsync(string conversationId, int limit = 50, DateTime? before = null)
    {
        var query = _collection
            .Find(m => m.ConversationId == conversationId && !m.IsDeleted && (before == null || m.CreatedAt < before))
            .SortByDescending(m => m.CreatedAt)
            .Limit(limit);

        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Message>> FindOlderThanAsync(DateTime olderThan)
    {
        return await FindAsync(m => m.CreatedAt < olderThan && !m.IsDeleted);
    }
}
