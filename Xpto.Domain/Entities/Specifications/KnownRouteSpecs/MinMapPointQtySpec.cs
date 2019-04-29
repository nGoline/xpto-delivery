using Xpto.Domain.Interfaces.Specification;

namespace Xpto.Domain.Entities.Specifications.KnownRouteSpecs
{
    public class MinMapPointQtySpec : ISpecification<KnownRoute>
    {
        public bool IsSatisfiedBy(KnownRoute knownRoute)
        {
            return knownRoute.MapPointIds.Count >= 2;
        }
    }
}