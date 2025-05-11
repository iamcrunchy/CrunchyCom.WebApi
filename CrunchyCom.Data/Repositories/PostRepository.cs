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
    public async Task<IEnumerable<Post>> GetAll()
    {
        _logger.LogInformation("Retrieving all posts");
        try
        {
            var posts = await _collection.Find(_ => true)
                .Project(p => new Post()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Author = p.Author,
                    PublishedDate = p.PublishedDate,
                    Tags = p.Tags
                    
                })
                .ToListAsync();
            _logger.LogInformation($"Retrieved {posts.Count} posts");
            return posts;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving ALL posts");
            throw;
        }
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