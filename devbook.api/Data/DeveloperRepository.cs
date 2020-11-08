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

            if(userParams.Favouriters){
                var userFavouriters = await GetUserFavourites(userParams.UserId, userParams.Favouriters);
                user = user.Where( u => userFavouriters.Contains(u.Id));
                
            }

            if(userParams.Favouritees){
                var userFavouritees = await GetUserFavourites(userParams.UserId, userParams.Favouriters);
                user = user.Where( u => userFavouritees.Contains(u.Id));
                
                
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


        private async Task<IEnumerable<int>> GetUserFavourites(int id, bool favouriters)
        {
            var user = await _context.Users
                .Include(x => x.Favouriters)
                .Include(x => x.Favouritees)
                .FirstOrDefaultAsync(u => u.Id == id);
            if(favouriters){
                return user.Favouriters.Where(u => u.FavouriteeId == id).Select(i => i.FavouriterId);
            }
            else{
                return user.Favouritees.Where(u => u.FavouriterId == id).Select(i => i.FavouriteeId);
            }
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

        public async Task<Favourite> GetFavourite(int userId, int recipientId)
        {
                return await _context.Favourite.FirstOrDefaultAsync( u =>
                u.FavouriterId == userId && u.FavouriteeId == recipientId);
            
        }

        public async Task<Messages> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<Messages>> GetMessagesForUser(MessageParams messageParams)
        {
            var messages = _context.Messages
                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .AsQueryable();


            switch(messageParams.MesssageContainer)
            {
                case "Inbox":
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId && u.RecipientDelete == false);
                    break;
                case "Outbox":
                    messages = messages.Where(u => u.SenderId == messageParams.UserId && u.SenderDeleted == false);
                    break;
                default:
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId 
                    && u.RecipientDelete == false  && u.IsRead == false);
                    break;
                

            }
            messages = messages.OrderByDescending(d => d.MessageSent);
            return await PagedList<Messages>.CreateAsync(messages,messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Messages>> GetMessageThread(int userId, int recipientId)
        {
            var messages = await _context.Messages
            .Include(u => u.Sender).ThenInclude(p => p.Photos)
            .Include(u => u.Recipient).ThenInclude(p => p.Photos)
            .Where(m => m.RecipientId == userId && m.RecipientDelete == false
                    && m.SenderId == recipientId
                    || m.RecipientId == recipientId && m.SenderId == userId
                    && m.SenderDeleted == false)
            .OrderByDescending(m => m.MessageSent)
            .ToListAsync();

                return messages;
    }
    }
}