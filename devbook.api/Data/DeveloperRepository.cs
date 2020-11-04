using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using devbook.api.Helpers;
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

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {

            var user =   _context.Users.Include(p => p.Photos).Include(s => s.Skills).OrderByDescending(u => u.LastActive).AsQueryable();
            user = user.Where(u => u.Id != userParams.UserId);
            if(userParams.Skill != null){
            var skillSearch = await GetSkillBySkill(userParams.Skill.ToLower());
            user = user.Where( u => skillSearch.Contains(u.Id));
                
            }



            if (!string.IsNullOrEmpty(userParams.OrderBy)){
                switch(userParams.OrderBy){
                    case "created":
                        user = user.OrderByDescending(u => u.Created);
                        break;
                    default:
                        user = user = user.OrderByDescending(u => u.LastActive);
                        break;
                }
            }





          
            return await PagedList<User>.CreateAsync(user, userParams.PageNumber, userParams.PageSize);
            
        }
        
        public async Task<Skills> GetSkill(int id)
        {
            var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == id);
            return skill;
        }

        public async Task<IEnumerable<int>> GetSkillBySkill(string searchSkill)
        {
    
            return _context.Skills.Where(s => s.Skill.ToLower() == searchSkill).Select(i => i.UserId);

        }
            
        











        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<Skills> addSkill(Skills skill)
        {
            await _context.Skills.AddAsync(skill);
            await _context.SaveChangesAsync();
            return skill;
        }
        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
            return photo;
        }

       public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos
                .Where(u => u.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);
        }
    }
}