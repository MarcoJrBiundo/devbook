using System.Collections.Generic;
using System.Threading.Tasks;
using devbook.api.models;
using Microsoft.EntityFrameworkCore;

namespace devbook.api.Data
{
    public class DeveloperRepository : IDeveloperRepository
    {
        private readonly DataContext _context;

        public DeveloperRepository(DataContext context)
        {
            this._context = context;
            
        }
        public void Add<T>(T entity) where T : class
        {
             _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
             _context.Remove(entity);
        }

        public async Task<User> GetUser(int id)
        {
           var user = await  _context.Users.Include(p => p.Photos).Include(s => s.Skills).FirstOrDefaultAsync(u => u.Id == id);
           return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {

            var user = await  _context.Users.Include(p => p.Photos).Include(s => s.Skills).ToListAsync();
            return user;
            
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}