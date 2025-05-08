namespace CrunchyCom.Data.Config;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string PostsCollection { get; set; } = null!;
    public string UsersCollection { get; set; } = null!;
}