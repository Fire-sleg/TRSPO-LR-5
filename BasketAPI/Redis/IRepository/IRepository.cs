using BasketAPI.Models;
using System.Linq.Expressions;

namespace BasketAPI.Redis.IRepository
{
    public interface IRepository
    {
        Task<List<Product>> GetAllAsync(); //Expression<Func<T, bool>>? filter = null
        Task<Product> GetAsync(Guid id, bool tracked = true);
        Task CreateAsync(Product entity);
        Task RemoveAsync(Product entity);
        Task SaveAsync();
    }
}
