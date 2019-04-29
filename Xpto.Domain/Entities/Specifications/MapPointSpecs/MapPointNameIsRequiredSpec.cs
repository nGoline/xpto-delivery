using Xpto.Domain.Interfaces.Specification;

namespace Xpto.Domain.Entities.Specifications.MapPointSpecs
{
    public class MapPointNameIsRequiredSpec : ISpecification<MapPoint>
    {
        public bool IsSatisfiedBy(MapPoint mapPoint)
        {
            return !string.IsNullOrWhiteSpace(mapPoint.Name);
        }
    }
}