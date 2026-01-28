namespace TelegramLite.Infrastructure.Data.MongoDB;

/// <summary>
/// Configuration settings for MongoDB connection.
/// </summary>
public class MongoDbSettings
{
    /// <summary>
    /// Gets or sets the MongoDB connection string.
    /// </summary>
    public string ConnectionString { get; set; } = "mongodb://localhost:27017";

    /// <summary>
    /// Gets or sets the database name.
    /// </summary>
    public string DatabaseName { get; set; } = "telegramlite";
}
