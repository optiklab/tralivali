using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Tralivali.Core.Models;

public class Conversation
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    
    public List<string> ParticipantIds { get; set; } = new();
    public string LastMessageId { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
