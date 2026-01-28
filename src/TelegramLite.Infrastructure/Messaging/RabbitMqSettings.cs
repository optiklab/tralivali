namespace TelegramLite.Infrastructure.Messaging;

/// <summary>
/// Configuration settings for RabbitMQ connection.
/// </summary>
public class RabbitMqSettings
{
    /// <summary>
    /// Gets or sets the RabbitMQ connection string.
    /// </summary>
    public string ConnectionString { get; set; } = "amqp://localhost:5672";

    /// <summary>
    /// Gets or sets the exchange name.
    /// </summary>
    public string ExchangeName { get; set; } = "telegramlite.messages";

    /// <summary>
    /// Gets or sets the exchange type.
    /// </summary>
    public string ExchangeType { get; set; } = "topic";

    /// <summary>
    /// Gets or sets the message processing queue name.
    /// </summary>
    public string MessagesQueue { get; set; } = "messages.process";

    /// <summary>
    /// Gets or sets the file processing queue name.
    /// </summary>
    public string FilesQueue { get; set; } = "files.process";

    /// <summary>
    /// Gets or sets the archival processing queue name.
    /// </summary>
    public string ArchivalQueue { get; set; } = "archival.process";

    /// <summary>
    /// Gets or sets the backup processing queue name.
    /// </summary>
    public string BackupQueue { get; set; } = "backup.process";
}
