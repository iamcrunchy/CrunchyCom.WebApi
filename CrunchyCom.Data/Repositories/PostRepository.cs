using CrunchyCom.Data.Config;
using CrunchyCom.Data.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace CrunchyCom.Data.Repositories;

public class PostRepository : IRepository<Post>
{
    private readonly IMongoCollection<Post> _collection;
    private readonly ILogger<PostRepository> _logger;

    public PostRepository(
        IMongoClient mongoClient,
        MongoDbSettings settings,
        ILogger<PostRepository> logger)
    {
        _logger = logger;
        var database = mongoClient.GetDatabase(settings.DatabaseName);
        _collection = database.GetCollection<Post>(settings.PostsCollection);
    }

    // ... rest of repository implementation
    public IEnumerable<Post> GetAll()
    {
        throw new NotImplementedException();
    }

    public Post GetById(string id)
    {
        throw new NotImplementedException();
    }

    public void Add(Post entity)
    {
        throw new NotImplementedException();
    }

    public void Update(Post entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(string id)
    {
        throw new NotImplementedException();
    }
}