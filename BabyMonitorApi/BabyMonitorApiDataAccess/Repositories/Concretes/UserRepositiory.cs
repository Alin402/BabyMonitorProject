using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiDataAccess.CustomValidationExceptions;
using BabyMonitorApiDataAccess.Entities;
using BabyMonitorApiDataAccess.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BabyMonitorApiDataAccess.Repositories.Concretes
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(BabyMonitorContext context) : base(context)
        {
        }

        public async override Task<User> PostAsync(User user)
        {
            User? foundUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (foundUser != null)
            {
                throw new EmailAlreadyInUseException("Email already in use");
            }

            foundUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);

            if (foundUser != null)
            {
                throw new UsernameAlreadyInUseException("Username already in use");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            User? foundUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (foundUser == null)
            {
                throw new UserNotFoundException("User not found");
            }

            return foundUser;
        }

        public override async Task<User?> GetByIdAsync(Guid? id)
        {
            try
            {
                User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    throw new UserNotFoundException("User not found");
                }
                return user;
            }
            catch (UserNotFoundException)
            {
                throw;
            }
        }
    }
}
