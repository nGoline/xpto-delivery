using System.ComponentModel.DataAnnotations;
using Xpto.Domain.Interfaces.Specification;

namespace Xpto.Domain.Entities.Specifications.UserSpecs
{
    public class UserEmailMustBeValidSpec : ISpecification<User>
    {
        public bool IsSatisfiedBy(User user)
        {
            return new EmailAddressAttribute().IsValid(user.Email);
        }
    }
}