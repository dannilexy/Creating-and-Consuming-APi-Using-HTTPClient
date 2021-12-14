using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParkyWeb.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(string url, int id);
        Task<IEnumerable<T>> GetAllAsync(string url);
        Task<bool> CreateAsync(string url, T entity);
        Task<bool> UpdateAsync(string url, T entity);
        Task<bool> DeleteAsync(string url, int id);
    }
}
