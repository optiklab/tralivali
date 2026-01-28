using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TelegramLite.Infrastructure.Messaging;

/// <summary>
/// RabbitMQ service for message publishing and consumption with connection resilience.
/// </summary>
public class RabbitMqService : IMessagePublisher, IMessageConsumer, IDisposable
{
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<RabbitMqService> _logger;
    private readonly ResiliencePipeline _resiliencePipeline;
    private IConnection? _connection;
    private IChannel? _channel;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="RabbitMqService"/> class.
    /// </summary>
    /// <param name="settings">The RabbitMQ settings.</param>
    /// <param name="logger">The logger instance.</param>
    public RabbitMqService(IOptions<RabbitMqSettings> settings, ILogger<RabbitMqService> logger)
    {
        _settings = settings.Value;
        _logger = logger;

        // Create a resilience pipeline with retry policy
        _resiliencePipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(2),
                BackoffType = DelayBackoffType.Exponential,
                OnRetry = args =>
                {
                    _logger.LogWarning("Retry attempt {AttemptNumber} after {Delay}ms due to: {Exception}",
                        args.AttemptNumber, args.RetryDelay.TotalMilliseconds, args.Outcome.Exception?.Message);
                    return default;
                }
            })
            .Build();
    }

    /// <summary>
    /// Ensures a connection to RabbitMQ is established.
    /// </summary>
    private async Task EnsureConnectionAsync()
    {
        if (_connection != null && _connection.IsOpen)
        {
            return;
        }

        await _resiliencePipeline.ExecuteAsync(async token =>
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_settings.ConnectionString),
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            // Declare exchange
            await _channel.ExchangeDeclareAsync(
                exchange: _settings.ExchangeName,
                type: _settings.ExchangeType,
                durable: true,
                autoDelete: false);

            // Declare queues
            await DeclareQueuesAsync();

            _logger.LogInformation("Connected to RabbitMQ at {ConnectionString}", _settings.ConnectionString);
        }, CancellationToken.None);
    }

    /// <summary>
    /// Declares all required queues.
    /// </summary>
    private async Task DeclareQueuesAsync()
    {
        if (_channel == null) return;

        var queues = new[]
        {
            _settings.MessagesQueue,
            _settings.FilesQueue,
            _settings.ArchivalQueue,
            _settings.BackupQueue
        };

        foreach (var queue in queues)
        {
            await _channel.QueueDeclareAsync(
                queue: queue,
                durable: true,
                exclusive: false,
                autoDelete: false);

            // Bind queue to exchange with routing key = queue name
            await _channel.QueueBindAsync(
                queue: queue,
                exchange: _settings.ExchangeName,
                routingKey: queue);

            // Declare dead letter queue
            var dlqName = $"{queue}.dlq";
            await _channel.QueueDeclareAsync(
                queue: dlqName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            _logger.LogInformation("Declared queue {QueueName} and dead letter queue {DlqName}", queue, dlqName);
        }
    }

    /// <inheritdoc/>
    public async Task PublishAsync<T>(T message, string routingKey)
    {
        await EnsureConnectionAsync();

        if (_channel == null)
        {
            throw new InvalidOperationException("Channel is not initialized");
        }

        await _resiliencePipeline.ExecuteAsync(async token =>
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json",
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };

            await _channel!.BasicPublishAsync(
                exchange: _settings.ExchangeName,
                routingKey: routingKey,
                mandatory: true,
                basicProperties: properties,
                body: body);

            _logger.LogDebug("Published message to {RoutingKey}: {Message}", routingKey, json);
        }, CancellationToken.None);
    }

    /// <inheritdoc/>
    public async Task ConsumeAsync<T>(string queueName, Func<T, Task<bool>> handler)
    {
        await EnsureConnectionAsync();

        if (_channel == null)
        {
            throw new InvalidOperationException("Channel is not initialized");
        }

        var consumer = new AsyncEventingBasicConsumer(_channel);
        
        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<T>(json);

                if (message == null)
                {
                    _logger.LogWarning("Failed to deserialize message from queue {QueueName}", queueName);
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, false);
                    return;
                }

                var success = await handler(message);

                if (success)
                {
                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                    _logger.LogDebug("Successfully processed message from queue {QueueName}", queueName);
                }
                else
                {
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
                    _logger.LogWarning("Failed to process message from queue {QueueName}, requeueing", queueName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message from queue {QueueName}", queueName);
                await _channel.BasicNackAsync(ea.DeliveryTag, false, false);
            }
        };

        await _channel.BasicConsumeAsync(queueName, false, consumer);
        _logger.LogInformation("Started consuming from queue {QueueName}", queueName);
    }

    /// <inheritdoc/>
    public void StopConsuming()
    {
        _logger.LogInformation("Stopping message consumption");
        Dispose();
    }

    /// <summary>
    /// Disposes of the RabbitMQ connection and channel.
    /// </summary>
    public void Dispose()
    {
        if (_disposed) return;

        _channel?.Dispose();
        _connection?.Dispose();
        _disposed = true;

        _logger.LogInformation("RabbitMQ connection disposed");
    }
}
