namespace TelegramLite.Domain.Entities;

/// <summary>
/// Represents a backup of the TelegramLite system data.
/// </summary>
public class Backup
{
    /// <summary>
    /// Gets or sets the unique identifier for the backup.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the backup was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the collections included in this backup.
    /// </summary>
    public List<string> Collections { get; set; } = new();

    /// <summary>
    /// Gets or sets the blob storage paths for backup files.
    /// </summary>
    public Dictionary<string, string> BlobPaths { get; set; } = new();

    /// <summary>
    /// Gets or sets the total size of the backup in bytes.
    /// </summary>
    public long TotalSize { get; set; }

    /// <summary>
    /// Gets or sets the status of the backup.
    /// </summary>
    public BackupStatus Status { get; set; } = BackupStatus.InProgress;

    /// <summary>
    /// Gets or sets the timestamp when the backup was completed.
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Gets or sets any error message if the backup failed.
    /// </summary>
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// Represents the status of a backup.
/// </summary>
public enum BackupStatus
{
    /// <summary>
    /// Backup is in progress.
    /// </summary>
    InProgress = 0,

    /// <summary>
    /// Backup completed successfully.
    /// </summary>
    Completed = 1,

    /// <summary>
    /// Backup failed.
    /// </summary>
    Failed = 2
}
