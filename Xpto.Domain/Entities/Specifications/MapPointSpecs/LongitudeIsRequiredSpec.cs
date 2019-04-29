using Xpto.Domain.Interfaces.Specification;

namespace Xpto.Domain.Entities.Specifications.MapPointSpecs
{
    public class LongitudeIsRequiredSpec : ISpecification<MapPoint>
    {
        public bool IsSatisfiedBy(MapPoint mapPoint)
        {
            return mapPoint.Longitude != 0;
        }
    }
}