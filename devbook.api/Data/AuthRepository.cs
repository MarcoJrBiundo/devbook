using System.Threading.Tasks;
using devbook.api.models;
using Microsoft.EntityFrameworkCore;

namespace devbook.api.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context){_context = context; }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using( var hmac = new System.Security.Cryptography.HMACSHA512()){
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
           using( var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)){
              var computedHash  = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
              for(int i = 0; i < computedHash.Length; i++)
                  if(computedHash[i] != passwordHash[i]) return false;
            }
            return true;
        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Username == username);
            if (user == null || !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;
            return user;  
        }
        public async Task<User> Register(User user, string password)
        {
            byte[] passHash, passSalt;
            CreatePasswordHash(password, out passHash, out passSalt);
            user.PasswordHash = passHash;
            user.PasswordSalt = passSalt;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<bool> UserExists(string username)
        {
            if( await _context.Users.AnyAsync(x => x.Username == username))
                return true;
            return false;
        }
       
    }
}