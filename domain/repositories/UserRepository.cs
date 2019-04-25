using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using domain.contexts;
using domain.entities;
using domain.repositories;
using Microsoft.EntityFrameworkCore;

namespace domain.repositories
{
    public class UserRepository : RepositoryBase<UserEntity>
    {
        /// <summary>
        /// Repository constructor takes Context needed from IoC injection 
        /// </summary>
        /// <param name="context">DbContext object</param>
        public UserRepository(MainContext context)
            : base(context)
        {
        }

        public async Task<UserEntity> FindByUsernameAndPasswordAsync(string username, string password){
            return await DbSet.FirstOrDefaultAsync(u => u.Email.Equals(username) 
                                                     && u.Password.Equals(password));
        }

        // For testing purpose only
        internal async Task SeedDatabase()
        {
            await Context.Database.EnsureCreatedAsync();
        }
    }
}