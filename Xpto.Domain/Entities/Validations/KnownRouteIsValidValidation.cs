using Xpto.Domain.Entities.Specifications.KnownRouteSpecs;
using Xpto.Domain.Validation;

namespace Xpto.Domain.Entities.Validations
{
    public class KnownRouteIsValidValidation : Validation<KnownRoute>
    {
        public KnownRouteIsValidValidation()
        {
            base.AddRule(new ValidationRule<KnownRoute>(new MinMapPointQtySpec(), "From MapPoint is required."));
            base.AddRule(new ValidationRule<KnownRoute>(new CostIsRequiredSpec(), "Cost is required."));
            base.AddRule(new ValidationRule<KnownRoute>(new TimeIsRequiredSpec(), "Time is required."));
        }
    }
}