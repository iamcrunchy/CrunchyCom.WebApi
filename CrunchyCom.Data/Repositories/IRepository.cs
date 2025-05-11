using CrunchyCom.Data.Models;

namespace CrunchyCom.Data.Repositories;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    T GetById(string id);
    void Add(T entity);
    void Update(T entity);
    void Delete(string id);
}