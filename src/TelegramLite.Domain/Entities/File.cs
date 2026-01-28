namespace TelegramLite.Domain.Entities;

/// <summary>
/// Represents a file uploaded to the TelegramLite system.
/// </summary>
public class FileAttachment
{
    /// <summary>
    /// Gets or sets the unique identifier for the file.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the conversation ID this file belongs to.
    /// </summary>
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user ID of the uploader.
    /// </summary>
    public string UploaderId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the original filename.
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the MIME type of the file.
    /// </summary>
    public string MimeType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the size of the file in bytes.
    /// </summary>
    public long Size { get; set; }

    /// <summary>
    /// Gets or sets the blob storage path for the file.
    /// </summary>
    public string BlobPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the blob storage path for the thumbnail, if applicable.
    /// </summary>
    public string? ThumbnailPath { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the file was uploaded.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets metadata for the file (e.g., image dimensions, video duration).
    /// </summary>
    public FileMetadata? Metadata { get; set; }

    /// <summary>
    /// Gets or sets whether the file has been deleted.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the file was deleted, if applicable.
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}

/// <summary>
/// Represents metadata for a file.
/// </summary>
public class FileMetadata
{
    /// <summary>
    /// Gets or sets the width of an image or video, if applicable.
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of an image or video, if applicable.
    /// </summary>
    public int? Height { get; set; }

    /// <summary>
    /// Gets or sets the duration of a video or audio file in seconds, if applicable.
    /// </summary>
    public double? Duration { get; set; }

    /// <summary>
    /// Gets or sets EXIF metadata for images.
    /// </summary>
    public Dictionary<string, string>? ExifData { get; set; }
}
