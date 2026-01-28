using TelegramLite.Domain.Entities;
using TelegramLite.Infrastructure.Data.MongoDB;

namespace TelegramLite.Infrastructure.Data.Repositories;

/// <summary>
/// Repository for Conversation entities.
/// </summary>
public interface IConversationRepository : IRepository<Conversation>
{
    /// <summary>
    /// Finds conversations for a specific user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>A list of conversations the user is participating in.</returns>
    Task<IEnumerable<Conversation>> FindByUserIdAsync(string userId);

    /// <summary>
    /// Finds a direct conversation between two users.
    /// </summary>
    /// <param name="userId1">The first user ID.</param>
    /// <param name="userId2">The second user ID.</param>
    /// <returns>The direct conversation if found, otherwise null.</returns>
    Task<Conversation?> FindDirectConversationAsync(string userId1, string userId2);
}

/// <summary>
/// MongoDB implementation of IConversationRepository.
/// </summary>
public class ConversationRepository : MongoRepository<Conversation>, IConversationRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConversationRepository"/> class.
    /// </summary>
    /// <param name="context">The MongoDB context.</param>
    public ConversationRepository(MongoDbContext context) : base(context.Conversations)
    {
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Conversation>> FindByUserIdAsync(string userId)
    {
        return await FindAsync(c => c.Participants.Any(p => p.UserId == userId));
    }

    /// <inheritdoc/>
    public async Task<Conversation?> FindDirectConversationAsync(string userId1, string userId2)
    {
        return await FindOneAsync(c => 
            c.Type == ConversationType.Direct && 
            c.Participants.Any(p => p.UserId == userId1) &&
            c.Participants.Any(p => p.UserId == userId2));
    }
}
