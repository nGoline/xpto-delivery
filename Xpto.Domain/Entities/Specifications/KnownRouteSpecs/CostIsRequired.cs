using System;
using System.Security.AccessControl;
using Xpto.Domain.Interfaces.Specification;

namespace Xpto.Domain.Entities.Specifications.KnownRouteSpecs
{
    public class CostIsRequiredSpec : ISpecification<KnownRoute>
    {
        public bool IsSatisfiedBy(KnownRoute route){
            return route.Cost != 0;
        }
    }
}