namespace TelegramLite.Infrastructure.Messaging;

/// <summary>
/// Interface for publishing messages to a message queue.
/// </summary>
public interface IMessagePublisher
{
    /// <summary>
    /// Publishes a message to the specified routing key.
    /// </summary>
    /// <typeparam name="T">The message type.</typeparam>
    /// <param name="message">The message to publish.</param>
    /// <param name="routingKey">The routing key for the message.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PublishAsync<T>(T message, string routingKey);
}

/// <summary>
/// Interface for consuming messages from a message queue.
/// </summary>
public interface IMessageConsumer
{
    /// <summary>
    /// Starts consuming messages from the specified queue.
    /// </summary>
    /// <typeparam name="T">The message type.</typeparam>
    /// <param name="queueName">The queue name to consume from.</param>
    /// <param name="handler">The message handler function.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ConsumeAsync<T>(string queueName, Func<T, Task<bool>> handler);

    /// <summary>
    /// Stops consuming messages.
    /// </summary>
    void StopConsuming();
}
