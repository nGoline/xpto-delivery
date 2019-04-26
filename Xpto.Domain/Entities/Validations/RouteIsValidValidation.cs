using Xpto.Domain.Entities.Specifications;
using Xpto.Domain.Validation;

namespace Xpto.Domain.Entities.Validations
{
    public class RouteIsValidValidation : Validation<Route>
    {
        public RouteIsValidValidation()
        {
            base.AddRule(new ValidationRule<Route>(new FromMapPointIsRequiredSpec(), "From MapPoint is required."));
            base.AddRule(new ValidationRule<Route>(new ToMapPointIsRequiredSpec(), "To MapPoint is required."));
        }
    }
}