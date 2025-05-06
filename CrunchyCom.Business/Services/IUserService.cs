using CrunchyCom.Data.Models;

namespace CrunchyCom.Business.Services;

public interface IUserService
{
    IEnumerable<User> GetAllUsers();
    User GetUserById(string id);
    void CreateUser(User user);
    void UpdateUser(User user);
    void DeleteUser(string id);
}