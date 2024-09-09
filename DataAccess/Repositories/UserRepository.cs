using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<Guid?> GetUserIdByUsernameAsync(string username)
        {
            var user = await Context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return null; 
            }

            return user.Id;
        }
    }
}
