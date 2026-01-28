using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Tralivali.Core.Models;

public class Message
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    
    public string SenderId { get; set; } = string.Empty;
    public string RecipientId { get; set; } = string.Empty;
    public string ConversationId { get; set; } = string.Empty;
    
    public string EncryptedContent { get; set; } = string.Empty;
    public string EncryptedKey { get; set; } = string.Empty;
    
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeliveredAt { get; set; }
    public DateTime? ReadAt { get; set; }
    
    public bool IsDeleted { get; set; } = false;
}
