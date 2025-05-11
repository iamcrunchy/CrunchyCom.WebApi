using CrunchyCom.Data.Models;
using CrunchyCom.Data.Repositories;

namespace CrunchyCom.Business.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;

    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public IEnumerable<User> GetAllUsers()
    {
        throw new NotImplementedException();
    }

    public User GetUserById(string id)
    {
        return _userRepository.GetById(id);
    }

    public void CreateUser(User user)
    {
        _userRepository.Add(user);
    }

    public void UpdateUser(User user)
    {
        _userRepository.Update(user);
    }

    public void DeleteUser(string id)
    {
        _userRepository.Delete(id);
    }
}