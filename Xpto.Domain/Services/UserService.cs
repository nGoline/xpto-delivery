using System.Linq;
using System.Threading.Tasks;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Repository;
using Xpto.Domain.Interfaces.Service;
using Xpto.Domain.Interfaces.Validation;
using Xpto.Domain.Services.Common;
using Xpto.Domain.Validation;

namespace Xpto.Domain.Services
{
    public class UserService : Service<User>, IUserService
    {
        public UserService(IUserRepository repository)
            : base(repository)
        {
        }

        public async Task<(User, ValidationResult)> AuthenticateAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                ValidationResult.Add("Password can't be null");
                return (user, ValidationResult);
            }

            if (!ValidationResult.IsValid)
                return (user, ValidationResult);

            var selfValidationEntity = user as ISelfValidation;
            if (selfValidationEntity != null && !selfValidationEntity.IsValid)
                return (user, selfValidationEntity.ValidationResult);

            var users = await Repository.FindAsync(u => u.Email.Equals(user.Email) && u.Password.Equals(user.Password));
            var foundUser = users.SingleOrDefault();
            if (foundUser == null)
            {
                user = null;
                ValidationResult.Add("User not found for the provided combination of 'email' and 'password'");
            }
            else
            {
                user.Id = foundUser.Id;
                user.Name = foundUser.Name;
            }

            return (user, ValidationResult);
        }
    }
}