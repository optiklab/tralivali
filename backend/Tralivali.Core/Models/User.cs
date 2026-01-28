using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Tralivali.Core.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string PublicKey { get; set; } = string.Empty;
    public string PrivateKeyEncrypted { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    
    public bool IsActive { get; set; } = true;
    public List<string> InvitedBy { get; set; } = new();
    public List<string> InvitedUsers { get; set; } = new();
}
