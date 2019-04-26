using Xpto.Domain.Interfaces.Specification;

namespace Xpto.Domain.Entities.Specifications
{
    public class LatitudeMustBeValidSpec : ISpecification<MapPoint>
    {
        public bool IsSatisfiedBy(MapPoint mapPoint)
        {
            return mapPoint.Latitude >= -90 && mapPoint.Latitude <= 90;
        }
    }
}