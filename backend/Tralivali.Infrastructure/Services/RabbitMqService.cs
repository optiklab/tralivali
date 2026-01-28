using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using Tralivali.Infrastructure.Configuration;

namespace Tralivali.Infrastructure.Services;

public class RabbitMqService : IDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;

    public RabbitMqService(IOptions<RabbitMqSettings> settings)
    {
        var factory = new ConnectionFactory
        {
            HostName = settings.Value.HostName,
            Port = settings.Value.Port,
            UserName = settings.Value.UserName,
            Password = settings.Value.Password,
            VirtualHost = settings.Value.VirtualHost
        };

        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
    }

    public IChannel GetChannel() => _channel;

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
