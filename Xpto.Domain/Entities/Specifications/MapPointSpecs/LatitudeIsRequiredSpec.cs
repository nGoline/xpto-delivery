using Xpto.Domain.Interfaces.Specification;

namespace Xpto.Domain.Entities.Specifications.MapPointSpecs
{
    public class LatitudeIsRequiredSpec : ISpecification<MapPoint>
    {
        public bool IsSatisfiedBy(MapPoint mapPoint)
        {
            return mapPoint.Latitude != 0;
        }
    }
}