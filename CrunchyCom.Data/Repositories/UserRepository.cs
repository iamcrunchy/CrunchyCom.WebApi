using CrunchyCom.Data.Config;
using CrunchyCom.Data.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace CrunchyCom.Data.Repositories;

public class UserRepository : MongoRepository<User>
{
    private readonly IMongoCollection<User> _collection;

    public UserRepository(MongoDbSettings settings, ILogger<UserRepository> logger)
        : base(settings, settings.UsersCollection, logger)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _collection = database.GetCollection<User>(settings.UsersCollection);
    }

    public User? GetByUserName(string userName)
    {
        var filter = Builders<User>.Filter.Eq(u => u.UserName, userName);
        return _collection.Find(filter).FirstOrDefault();
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
}