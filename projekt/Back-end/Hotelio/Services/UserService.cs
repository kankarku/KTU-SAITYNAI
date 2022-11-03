using Hotelio.Context;
using Hotelio.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotelio.Services
{
    public class UserService
    {
        private readonly CrudContext _context;

        public UserService(CrudContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> Get(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<Room?> GetRoom(int roomId) => (await _context.Rooms.Include(m => m.Residents).ToListAsync())
            .FirstOrDefault(x => x.Id == roomId);

        public async Task<bool> Update(int id, string? username, string? password, string? fullName, string? phoneNumber)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            if (username != null) user.UserName = username;
            //if (password != null) user.PasswordHash = password;
            if (phoneNumber != null) user.PhoneNumber = phoneNumber;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}