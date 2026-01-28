using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Tralivali.Core.Models;

public class Invite
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    
    public string InviterUserId { get; set; } = string.Empty;
    public string InviteCode { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }
    public DateTime? UsedAt { get; set; }
    
    public bool IsUsed { get; set; } = false;
    public string? UsedByUserId { get; set; }
}
