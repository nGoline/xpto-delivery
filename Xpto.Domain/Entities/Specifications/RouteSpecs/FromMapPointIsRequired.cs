using System;
using System.Security.AccessControl;
using Xpto.Domain.Interfaces.Specification;

namespace Xpto.Domain.Entities.Specifications
{
    public class FromMapPointIsRequiredSpec : ISpecification<Route>
    {
        public bool IsSatisfiedBy(Route route){
            return route.FromId != null && route.FromId != Guid.Empty;
        }
    }
}