using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TelegramLite.Domain.Entities;

namespace TelegramLite.Infrastructure.Data.MongoDB;

/// <summary>
/// MongoDB database context for TelegramLite.
/// </summary>
public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    private bool _indexesCreated;
    private readonly object _indexLock = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="MongoDbContext"/> class.
    /// </summary>
    /// <param name="settings">The MongoDB settings.</param>
    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    /// <summary>
    /// Ensures indexes are created. This method is thread-safe and idempotent.
    /// </summary>
    public async Task EnsureIndexesAsync()
    {
        if (_indexesCreated) return;

        lock (_indexLock)
        {
            if (_indexesCreated) return;
            
            // Create indexes synchronously in a locked context to prevent duplicate creation
            CreateIndexes();
            _indexesCreated = true;
        }
    }

    /// <summary>
    /// Gets the Users collection.
    /// </summary>
    public IMongoCollection<User> Users => _database.GetCollection<User>("users");

    /// <summary>
    /// Gets the Conversations collection.
    /// </summary>
    public IMongoCollection<Conversation> Conversations => _database.GetCollection<Conversation>("conversations");

    /// <summary>
    /// Gets the Messages collection.
    /// </summary>
    public IMongoCollection<Message> Messages => _database.GetCollection<Message>("messages");

    /// <summary>
    /// Gets the Invites collection.
    /// </summary>
    public IMongoCollection<Invite> Invites => _database.GetCollection<Invite>("invites");

    /// <summary>
    /// Gets the Files collection.
    /// </summary>
    public IMongoCollection<FileAttachment> Files => _database.GetCollection<FileAttachment>("files");

    /// <summary>
    /// Gets the Backups collection.
    /// </summary>
    public IMongoCollection<Backup> Backups => _database.GetCollection<Backup>("backups");

    private void CreateIndexes()
    {
        // Users indexes
        var usersIndexKeys = Builders<User>.IndexKeys.Ascending(u => u.Email);
        var usersIndexOptions = new CreateIndexOptions { Unique = true };
        Users.Indexes.CreateOne(new CreateIndexModel<User>(usersIndexKeys, usersIndexOptions));

        // Messages indexes
        var messagesCompoundKeys = Builders<Message>.IndexKeys
            .Ascending(m => m.ConversationId)
            .Descending(m => m.CreatedAt);
        Messages.Indexes.CreateOne(new CreateIndexModel<Message>(messagesCompoundKeys));

        // Conversations indexes for participants
        var conversationsParticipantsKeys = Builders<Conversation>.IndexKeys
            .Ascending("Participants.UserId")
            .Descending(c => c.LastMessageAt);
        Conversations.Indexes.CreateOne(new CreateIndexModel<Conversation>(conversationsParticipantsKeys));

        // Invites indexes with TTL
        var invitesTokenKeys = Builders<Invite>.IndexKeys.Ascending(i => i.Token);
        var invitesTokenOptions = new CreateIndexOptions { Unique = true };
        Invites.Indexes.CreateOne(new CreateIndexModel<Invite>(invitesTokenKeys, invitesTokenOptions));

        var invitesExpireKeys = Builders<Invite>.IndexKeys.Ascending(i => i.ExpiresAt);
        var invitesExpireOptions = new CreateIndexOptions { ExpireAfter = TimeSpan.Zero };
        Invites.Indexes.CreateOne(new CreateIndexModel<Invite>(invitesExpireKeys, invitesExpireOptions));
    }
}
