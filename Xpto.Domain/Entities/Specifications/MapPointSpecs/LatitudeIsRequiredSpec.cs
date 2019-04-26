using Xpto.Domain.Interfaces.Specification;

namespace Xpto.Domain.Entities.Specifications
{
    public class LatitudeIsRequiredSpec : ISpecification<MapPoint>
    {
        public bool IsSatisfiedBy(MapPoint mapPoint)
        {
            return mapPoint.Latitude != 0;
        }
    }
}