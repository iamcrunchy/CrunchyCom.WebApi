using CrunchyCom.Data.Config;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace CrunchyCom.Data.Repositories;

/// <summary>
///     Provides a repository implementation for MongoDB, using a specified model type and collection.
/// </summary>
/// <typeparam name="T">
///     The type of the objects to be stored and retrieved in the MongoDB collection.
///     Must be a reference type.
/// </typeparam>
public class MongoRepository<T> : IRepository<T> where T : class
{
    private readonly IMongoCollection<T> _collection;
    private readonly ILogger<MongoRepository<T>> _logger;

    /// <summary>
    ///     Represents a generic repository implementation for storing, retrieving,
    ///     updating, and deleting entities in a MongoDB collection.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the entities managed by the repository. Must be a reference type.
    /// </typeparam>
    protected MongoRepository(MongoDbSettings database,
        string collectionName,
        ILogger<MongoRepository<T>> logger)
    {
        var client = new MongoClient(database.ConnectionString);
        var mongoDatabase = client.GetDatabase(database.DatabaseName);
        _collection = mongoDatabase.GetCollection<T>(collectionName);
        _logger = logger;
    }

    /// <summary>
    ///     Retrieves all entities from the MongoDB collection.
    /// </summary>
    /// <returns>
    ///     An enumerable collection of entities of type T from the MongoDB collection.
    /// </returns>
    public async Task<IEnumerable<T>> GetAll()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    /// <summary>
    ///     Retrieves an entity from the MongoDB collection based on its unique identifier.
    /// </summary>
    /// <param name="id">
    ///     The unique identifier of the entity to retrieve.
    /// </param>
    /// <returns>
    ///     The entity of type T with the specified identifier, or null if no entity is found.
    /// </returns>
    public T GetById(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        return _collection.Find(filter).FirstOrDefault();
    }

    /// <summary>
    ///     Adds a new entity of type T to the MongoDB collection.
    /// </summary>
    /// <param name="entity">
    ///     The entity of type T to be added to the collection. Must not be null.
    /// </param>
    public void Add(T entity)
    {
        _collection.InsertOne(entity);
    }

    /// <summary>
    ///     Updates an existing entity in the MongoDB collection by replacing it with a new entity.
    ///     The method identifies the entity to update using the "Id" property of the provided entity.
    /// </summary>
    /// <param name="entity">
    ///     The entity to update in the MongoDB collection. The entity must have an "Id"
    ///     property used to locate the corresponding entity in the database.
    /// </param>
    public void Update(T entity)
    {
        var id = entity.GetType().GetProperty("Id")?.GetValue(entity)?.ToString();
        var filter = Builders<T>.Filter.Eq("_id", id);
        _collection.ReplaceOne(filter, entity);
    }

    /// <summary>
    ///     Deletes an entity from the MongoDB collection based on the specified unique identifier.
    /// </summary>
    /// <param name="id">
    ///     The unique identifier of the entity to be deleted from the MongoDB collection.
    /// </param>
    public void Delete(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        _collection.DeleteOne(filter);
    }
}