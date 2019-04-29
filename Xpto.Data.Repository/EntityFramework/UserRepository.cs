using Microsoft.EntityFrameworkCore;
using Xpto.Data.Context;
using Xpto.Data.Context.Interfaces;
using Xpto.Data.Repository.EntityFramework.Common;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Repository;

namespace Xpto.Data.Repository.EntityFramework
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(XptoContext context)
            : base(context)
        {
        #if DEBUG
            context.Database.EnsureCreated();
        #endif
        }
    }
}