namespace TelegramLite.Domain.Entities;

/// <summary>
/// Represents an invite token for registering new users.
/// </summary>
public class Invite
{
    /// <summary>
    /// Gets or sets the unique identifier for the invite.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the invite token (HMAC-SHA256 signed).
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user ID of the inviter.
    /// </summary>
    public string InviterId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the invite expires.
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the invite was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the user ID who used this invite, if any.
    /// </summary>
    public string? UsedBy { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the invite was used, if applicable.
    /// </summary>
    public DateTime? UsedAt { get; set; }

    /// <summary>
    /// Gets or sets whether the invite has been used.
    /// </summary>
    public bool IsUsed => UsedBy != null;

    /// <summary>
    /// Gets or sets whether the invite has expired.
    /// </summary>
    public bool IsExpired => DateTime.UtcNow > ExpiresAt;

    /// <summary>
    /// Gets whether the invite is valid (not used and not expired).
    /// </summary>
    public bool IsValid => !IsUsed && !IsExpired;
}
