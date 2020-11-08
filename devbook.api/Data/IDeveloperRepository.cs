using System.Collections.Generic;
using System.Threading.Tasks;
using devbook.api.Helpers;
using devbook.api.models;


namespace devbook.api.Data
{
    public interface IDeveloperRepository
    {
        void Add<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        Task<bool> SaveAll();
        Task<User> GetUser(int id);
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<Skills> GetSkill(int id);

        Task<Skills> addSkill(Skills skill);
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetMainPhotoForUser(int userId);
        Task<Favourite> GetFavourite(int userId, int recipientId);

   
        Task<Messages> GetMessage(int id);
        Task<PagedList<Messages>> GetMessagesForUser(MessageParams messageParams);   
        Task<IEnumerable<Messages>> GetMessageThread(int userId, int recipientId);   



        
   
    }
}