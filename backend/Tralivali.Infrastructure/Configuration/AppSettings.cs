namespace Tralivali.Infrastructure.Configuration;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = "mongodb://localhost:27017";
    public string DatabaseName { get; set; } = "tralivali";
}

public class RabbitMqSettings
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
}

public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = "tralivali";
    public string Audience { get; set; } = "tralivali-users";
    public int ExpirationMinutes { get; set; } = 1440; // 24 hours
}

public class AzureCommunicationSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string SenderEmail { get; set; } = string.Empty;
}
