using System.Collections.Generic;
using System.Threading.Tasks;
using devbook.api.models;

namespace devbook.api.Data
{
    public interface IDeveloperRepository
    {
        void Add<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        Task<bool> SaveAll();
        Task<User> GetUser(int id);
         Task<IEnumerable<User>> GetUsers();
    }
}