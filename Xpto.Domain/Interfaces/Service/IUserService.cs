using System.Threading.Tasks;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Service.Common;

namespace Xpto.Domain.Interfaces.Service
{
    public interface IUserService : IService<User>
    {
        Task<User> AuthenticateAsync(string username, string password);
    }
}