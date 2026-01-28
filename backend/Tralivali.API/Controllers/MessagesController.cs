using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RabbitMQ.Client;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Tralivali.API.DTOs;
using Tralivali.Core.Models;
using Tralivali.Infrastructure.Services;

namespace Tralivali.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly MongoDbContext _mongoDb;
    private readonly RabbitMqService _rabbitMq;

    public MessagesController(MongoDbContext mongoDb, RabbitMqService rabbitMq)
    {
        _mongoDb = mongoDb;
        _rabbitMq = rabbitMq;
    }

    [HttpPost]
    public async Task<ActionResult<Message>> SendMessage([FromBody] SendMessageRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var messagesCollection = _mongoDb.GetCollection<Message>("messages");
        var conversationsCollection = _mongoDb.GetCollection<Conversation>("conversations");

        // Find or create conversation
        var conversation = await conversationsCollection
            .Find(c => c.ParticipantIds.Contains(userId) && c.ParticipantIds.Contains(request.RecipientId))
            .FirstOrDefaultAsync();

        if (conversation == null)
        {
            conversation = new Conversation
            {
                ParticipantIds = new List<string> { userId, request.RecipientId }
            };
            await conversationsCollection.InsertOneAsync(conversation);
        }

        var message = new Message
        {
            SenderId = userId,
            RecipientId = request.RecipientId,
            ConversationId = conversation.Id,
            EncryptedContent = request.EncryptedContent,
            EncryptedKey = request.EncryptedKey
        };

        await messagesCollection.InsertOneAsync(message);

        // Update conversation
        var update = Builders<Conversation>.Update
            .Set(c => c.LastMessageId, message.Id)
            .Set(c => c.UpdatedAt, DateTime.UtcNow);
        await conversationsCollection.UpdateOneAsync(c => c.Id == conversation.Id, update);

        // Publish to RabbitMQ for real-time delivery
        var channel = _rabbitMq.GetChannel();
        await channel.QueueDeclareAsync(
            queue: $"messages_{request.RecipientId}",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var messageJson = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(messageJson);

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: $"messages_{request.RecipientId}",
            body: body);

        return Ok(message);
    }

    [HttpGet("conversations/{conversationId}")]
    public async Task<ActionResult<List<Message>>> GetMessages(string conversationId, [FromQuery] int limit = 50)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var messagesCollection = _mongoDb.GetCollection<Message>("messages");
        var conversationsCollection = _mongoDb.GetCollection<Conversation>("conversations");

        // Verify user is participant
        var conversation = await conversationsCollection
            .Find(c => c.Id == conversationId && c.ParticipantIds.Contains(userId))
            .FirstOrDefaultAsync();

        if (conversation == null)
            return NotFound();

        var messages = await messagesCollection
            .Find(m => m.ConversationId == conversationId && !m.IsDeleted)
            .SortByDescending(m => m.SentAt)
            .Limit(limit)
            .ToListAsync();

        return Ok(messages);
    }

    [HttpGet("conversations")]
    public async Task<ActionResult<List<Conversation>>> GetConversations()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var conversationsCollection = _mongoDb.GetCollection<Conversation>("conversations");
        var conversations = await conversationsCollection
            .Find(c => c.ParticipantIds.Contains(userId))
            .SortByDescending(c => c.UpdatedAt)
            .ToListAsync();

        return Ok(conversations);
    }
}
