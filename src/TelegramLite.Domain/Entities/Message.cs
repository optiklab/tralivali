namespace TelegramLite.Domain.Entities;

/// <summary>
/// Represents a message in the TelegramLite system.
/// </summary>
public class Message
{
    /// <summary>
    /// Gets or sets the unique identifier for the message.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the conversation ID this message belongs to.
    /// </summary>
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user ID of the message sender.
    /// </summary>
    public string SenderId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of message.
    /// </summary>
    public MessageType Type { get; set; } = MessageType.Text;

    /// <summary>
    /// Gets or sets the plain text content of the message (for sender's cache only).
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the encrypted content of the message.
    /// </summary>
    public string? EncryptedContent { get; set; }

    /// <summary>
    /// Gets or sets the ID of the message this is a reply to, if any.
    /// </summary>
    public string? ReplyTo { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the message was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the list of user IDs who have read this message.
    /// </summary>
    public List<MessageRead> ReadBy { get; set; } = new();

    /// <summary>
    /// Gets or sets whether the message has been deleted.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the message was deleted, if applicable.
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Gets or sets file attachment information if this is a file message.
    /// </summary>
    public string? FileId { get; set; }
}

/// <summary>
/// Represents the type of message.
/// </summary>
public enum MessageType
{
    /// <summary>
    /// Plain text message.
    /// </summary>
    Text = 0,

    /// <summary>
    /// File attachment message.
    /// </summary>
    File = 1,

    /// <summary>
    /// System message (e.g., user joined, user left).
    /// </summary>
    System = 2
}

/// <summary>
/// Represents read receipt information for a message.
/// </summary>
public class MessageRead
{
    /// <summary>
    /// Gets or sets the user ID who read the message.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the message was read.
    /// </summary>
    public DateTime ReadAt { get; set; } = DateTime.UtcNow;
}
