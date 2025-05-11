using CrunchyCom.Data.Config;
using CrunchyCom.Data.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace CrunchyCom.Data.Repositories;

public class UserRepository : MongoRepository<User>
{
    private readonly IMongoCollection<User> _collection;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(MongoDbSettings settings, ILogger<UserRepository> logger)
        : base(settings, settings.UsersCollection, logger)
    {
        _logger = logger;
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _collection = database.GetCollection<User>(settings.UsersCollection);
    }

    public User? GetByUserName(string userName)
    {
        var filter = Builders<User>.Filter.Eq(u => u.UserName, userName);
        return _collection.Find(filter).FirstOrDefault();
    }

    /// <summary>
    /// Asynchronously retrieves a user from the database by their username.
    /// </summary>
    /// <param name="userName">
    /// The username of the user to retrieve.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains
    /// the user with the specified username, or null if no user is found.
    /// </returns>
    public async Task<User?> GetByUserNameAsync(string userName)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Email, userName);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    // public void Update(User entity)
    // {
    //     var user = GetById(entity.Id);
    //     if (user != null)
    //     {
    //         user.UserName = entity.UserName;
    //         user.Email = entity.Email;
    //     }
    // }
    
    public async Task CreateAsync(User user)
    {
        await _collection.InsertOneAsync(user);
    }
}