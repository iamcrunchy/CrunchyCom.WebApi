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

    public IEnumerable<User> GetAllUsers() => _userRepository.GetAll();

    public User GetUserById(int id) => _userRepository.GetById(id);

    public void CreateUser(User user) => _userRepository.Add(user);

    public void UpdateUser(User user) => _userRepository.Update(user);

    public void DeleteUser(int id) => _userRepository.Delete(id);
}