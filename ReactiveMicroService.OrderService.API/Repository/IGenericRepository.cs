using ReactiveMicroService.OrderService.API.Models;

namespace ReactiveMicroService.OrderService.API.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task<List<T>> GetAll();
        Task<T> Insert(T entity);
        Task<T> Update(T entity);
        Task Delete(int id, T entity);
        Task<List<T>> GetByColumns(Dictionary<string, object> filters);
    }
}