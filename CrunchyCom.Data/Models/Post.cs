namespace CrunchyCom.Data.Models;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Post
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Body { get; set; } = null!;
    public string Author { get; set; } = null!;
    public DateTime PublishedDate { get; set; }
    public DateTime ModifiedDate { get; set; } 
    public IEnumerable<string> Tags { get; set; } = null!;
}