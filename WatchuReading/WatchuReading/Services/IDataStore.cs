using System.Collections.Generic;
using System.Threading.Tasks;
using WhatchaReading.Models;

namespace WatchuReading
{
    public interface IDataStore<T> 
    {
        Task<bool> AddAsync(T item);
        Task<bool> UpdateAsync(T item);
        Task<bool> DeleteAsync(string id);
        Task<T> GetSingleAsync(string id);
        Task<IEnumerable<T>> GetAllAsync(bool forceRefresh = false);
        Task<User> LogInUserAsync(string username, string password);
    }
}
