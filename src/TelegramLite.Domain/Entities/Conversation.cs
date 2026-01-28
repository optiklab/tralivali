namespace TelegramLite.Domain.Entities;

/// <summary>
/// Represents a conversation in the TelegramLite system.
/// Can be either a direct conversation between two users or a group conversation.
/// </summary>
public class Conversation
{
    /// <summary>
    /// Gets or sets the unique identifier for the conversation.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of conversation (Direct or Group).
    /// </summary>
    public ConversationType Type { get; set; }

    /// <summary>
    /// Gets or sets the list of participants in the conversation.
    /// </summary>
    public List<Participant> Participants { get; set; } = new();

    /// <summary>
    /// Gets or sets the recent messages in the conversation (limited to 50).
    /// </summary>
    public List<Message> RecentMessages { get; set; } = new();

    /// <summary>
    /// Gets or sets the timestamp of the last message in the conversation.
    /// </summary>
    public DateTime LastMessageAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets additional metadata for the conversation (e.g., group name, avatar).
    /// </summary>
    public ConversationMetadata Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the timestamp when the conversation was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Represents the type of conversation.
/// </summary>
public enum ConversationType
{
    /// <summary>
    /// Direct conversation between two users.
    /// </summary>
    Direct = 0,

    /// <summary>
    /// Group conversation with multiple users.
    /// </summary>
    Group = 1
}

/// <summary>
/// Represents a participant in a conversation.
/// </summary>
public class Participant
{
    /// <summary>
    /// Gets or sets the user ID of the participant.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the role of the participant in the conversation.
    /// </summary>
    public ParticipantRole Role { get; set; } = ParticipantRole.Member;

    /// <summary>
    /// Gets or sets the timestamp when the participant joined the conversation.
    /// </summary>
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the timestamp of the last activity by this participant.
    /// </summary>
    public DateTime LastActivityAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Represents the role of a participant in a conversation.
/// </summary>
public enum ParticipantRole
{
    /// <summary>
    /// Regular member of the conversation.
    /// </summary>
    Member = 0,

    /// <summary>
    /// Administrator of the conversation with elevated permissions.
    /// </summary>
    Admin = 1,

    /// <summary>
    /// Owner/creator of the conversation with full permissions.
    /// </summary>
    Owner = 2
}

/// <summary>
/// Represents metadata for a conversation.
/// </summary>
public class ConversationMetadata
{
    /// <summary>
    /// Gets or sets the name of the conversation (for group conversations).
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the avatar URL for the conversation.
    /// </summary>
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// Gets or sets the description of the conversation.
    /// </summary>
    public string? Description { get; set; }
}
