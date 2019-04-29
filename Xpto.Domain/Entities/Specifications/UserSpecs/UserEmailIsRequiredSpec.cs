using Xpto.Domain.Interfaces.Specification;

namespace Xpto.Domain.Entities.Specifications.UserSpecs
{
    public class UserEmailIsRequiredSpec : ISpecification<User>
    {
        public bool IsSatisfiedBy(User user) => !string.IsNullOrWhiteSpace(user.Email);
    }
}