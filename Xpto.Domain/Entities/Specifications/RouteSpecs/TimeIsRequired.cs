using System;
using System.Security.AccessControl;
using Xpto.Domain.Interfaces.Specification;

namespace Xpto.Domain.Entities.Specifications.RouteSpecs
{
    public class TimeIsRequiredSpec : ISpecification<Route>
    {
        public bool IsSatisfiedBy(Route route){
            return route.Time != 0;
        }
    }
}