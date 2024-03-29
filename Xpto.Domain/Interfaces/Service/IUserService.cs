using System.Threading.Tasks;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Service.Common;
using Xpto.Domain.Validation;

namespace Xpto.Domain.Interfaces.Service
{
    public interface IUserService : IService<User>
    {
        Task<(User, ValidationResult)> AuthenticateAsync(User user);
    }
}