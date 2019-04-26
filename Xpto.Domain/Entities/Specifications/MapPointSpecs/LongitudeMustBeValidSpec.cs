using Xpto.Domain.Interfaces.Specification;

namespace Xpto.Domain.Entities.Specifications
{
    public class LongitudeMustBeValidSpec : ISpecification<MapPoint>
    {
        public bool IsSatisfiedBy(MapPoint mapPoint)
        {
            return mapPoint.Longitude >= -180 && mapPoint.Longitude <= 180;
        }
    }
}