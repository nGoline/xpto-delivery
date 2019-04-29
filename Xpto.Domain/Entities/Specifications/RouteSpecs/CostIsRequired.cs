using System;
using System.Security.AccessControl;
using Xpto.Domain.Interfaces.Specification;

namespace Xpto.Domain.Entities.Specifications.RouteSpecs
{
    public class CostIsRequiredSpec : ISpecification<Route>
    {
        public bool IsSatisfiedBy(Route route){
            return route.Cost != 0;
        }
    }
}