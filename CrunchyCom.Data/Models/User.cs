using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CrunchyCom.Data.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string PasswordHash { get; set; }
    public string? Picture { get; set; }
    public string Provider { get; set; } = null!; // "Google", etc.
    public string ProviderId { get; set; } = null!;
    public List<string> Roles { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}