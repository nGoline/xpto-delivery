using Xpto.Domain.Interfaces.Specification;

namespace Xpto.Domain.Entities.Specifications.UserSpecs
{
    public class UserNameIsRequiredSpec : ISpecification<User>
    {
        public bool IsSatisfiedBy(User user)
        {
            return !string.IsNullOrWhiteSpace(user.Name);
        }
    }
}