using System.Threading.Tasks;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Repository;
using Xpto.Domain.Interfaces.Service;
using Xpto.Domain.Services.Common;

namespace Xpto.Domain.Services
{
    public class UserService : Service<User>, IUserService
    {
        public UserService(IUserRepository repository) 
            : base(repository)
        {
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}