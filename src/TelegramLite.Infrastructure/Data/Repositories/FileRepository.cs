using TelegramLite.Domain.Entities;
using TelegramLite.Infrastructure.Data.MongoDB;

namespace TelegramLite.Infrastructure.Data.Repositories;

/// <summary>
/// Repository for FileAttachment entities.
/// </summary>
public interface IFileRepository : IRepository<FileAttachment>
{
    /// <summary>
    /// Finds files in a specific conversation.
    /// </summary>
    /// <param name="conversationId">The conversation ID.</param>
    /// <returns>A list of files in the conversation.</returns>
    Task<IEnumerable<FileAttachment>> FindByConversationIdAsync(string conversationId);

    /// <summary>
    /// Finds files uploaded by a specific user.
    /// </summary>
    /// <param name="uploaderId">The uploader's user ID.</param>
    /// <returns>A list of files uploaded by the user.</returns>
    Task<IEnumerable<FileAttachment>> FindByUploaderIdAsync(string uploaderId);
}

/// <summary>
/// MongoDB implementation of IFileRepository.
/// </summary>
public class FileRepository : MongoRepository<FileAttachment>, IFileRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileRepository"/> class.
    /// </summary>
    /// <param name="context">The MongoDB context.</param>
    public FileRepository(MongoDbContext context) : base(context.Files)
    {
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<FileAttachment>> FindByConversationIdAsync(string conversationId)
    {
        return await FindAsync(f => f.ConversationId == conversationId && !f.IsDeleted);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<FileAttachment>> FindByUploaderIdAsync(string uploaderId)
    {
        return await FindAsync(f => f.UploaderId == uploaderId && !f.IsDeleted);
    }
}
