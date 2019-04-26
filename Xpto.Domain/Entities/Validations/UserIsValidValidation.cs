using Xpto.Domain.Entities.Specifications;
using Xpto.Domain.Validation;

namespace Xpto.Domain.Entities.Validations
{
    public class UserIsValidValidation : Validation<User>
    {
        public UserIsValidValidation()
        {
            base.AddRule(new ValidationRule<User>(new UserNameIsRequiredSpec(), "Name is required."));
            base.AddRule(new ValidationRule<User>(new UserEmailIsRequiredSpec(), "Email is required."));
            base.AddRule(new ValidationRule<User>(new UserEmailMustBeValidSpec(), "Email is invalid."));
        }
    }
}