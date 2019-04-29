using Xpto.Domain.Entities.Specifications.MapPointSpecs;
using Xpto.Domain.Validation;

namespace Xpto.Domain.Entities.Validations
{
    public class MapPointIsValidValidation : Validation<MapPoint>
    {
        public MapPointIsValidValidation()
        {
            base.AddRule(new ValidationRule<MapPoint>(new MapPointNameIsRequiredSpec(), "Name is required."));
            base.AddRule(new ValidationRule<MapPoint>(new LatitudeIsRequiredSpec(), "Latitude is required."));
            base.AddRule(new ValidationRule<MapPoint>(new LatitudeMustBeValidSpec(), "Latitude should be between -90 and 90."));
            base.AddRule(new ValidationRule<MapPoint>(new LongitudeIsRequiredSpec(), "Longitude is required."));
            base.AddRule(new ValidationRule<MapPoint>(new LongitudeMustBeValidSpec(), "Longitude should be between -180 and 180."));
        }
    }
}