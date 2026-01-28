namespace TelegramLite.Domain.Entities;

/// <summary>
/// Represents a user in the TelegramLite system.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's display name.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the hashed password for the user.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's public key for end-to-end encryption.
    /// </summary>
    public string PublicKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of registered devices for this user.
    /// </summary>
    public List<Device> Devices { get; set; } = new();

    /// <summary>
    /// Gets or sets the timestamp when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the ID of the user who invited this user.
    /// </summary>
    public string? InvitedBy { get; set; }
}

/// <summary>
/// Represents a device registered to a user.
/// </summary>
public class Device
{
    /// <summary>
    /// Gets or sets the unique identifier for the device.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the device name or description.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the device was registered.
    /// </summary>
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the timestamp of the last activity from this device.
    /// </summary>
    public DateTime LastActiveAt { get; set; } = DateTime.UtcNow;
}
