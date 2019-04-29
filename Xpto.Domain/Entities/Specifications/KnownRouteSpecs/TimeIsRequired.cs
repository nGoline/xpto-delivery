using System;
using System.Security.AccessControl;
using Xpto.Domain.Interfaces.Specification;

namespace Xpto.Domain.Entities.Specifications.KnownRouteSpecs
{
    public class TimeIsRequiredSpec : ISpecification<KnownRoute>
    {
        public bool IsSatisfiedBy(KnownRoute route){
            return route.Time != 0;
        }
    }
}