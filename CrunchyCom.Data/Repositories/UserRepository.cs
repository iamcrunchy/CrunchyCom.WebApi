using CrunchyCom.Data.Models;

namespace CrunchyCom.Data.Repositories;

public class UserRepository : IRepository<User>
{
    private readonly List<User> _users = new();

    public IEnumerable<User> GetAll() => _users;

    public User GetById(string id) => _users.FirstOrDefault(u => u.Id == id)!;

    public void Add(User entity) => _users.Add(entity);

    public void Update(User entity)
    {
        var user = GetById(entity.Id);
        if (user != null)
        {
            user.Name = entity.Name;
            user.Email = entity.Email;
        }
    }

    public void Delete(string id) => _users.RemoveAll(u => u.Id == id);
}