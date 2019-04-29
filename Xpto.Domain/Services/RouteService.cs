using System.Security.AccessControl;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Repository;
using Xpto.Domain.Interfaces.Service;
using Xpto.Domain.Services.Common;
using System.Collections.Generic;
using Xpto.Domain.Validation;
using System.Linq.Expressions;

namespace Xpto.Domain.Services
{
    public class RouteService : Service<Route>, IRouteService
    {
        private IRouteRepository _repository;
        private IMapPointService _mapPointService;

        public RouteService(IRouteRepository repository, IMapPointService mapPointService)
            : base(repository)
        {
            _repository = repository;
            _mapPointService = mapPointService;
        }

        public override async Task<ValidationResult> AddAsync(Route route)
        {
            await base.AddAsync(route);

            return await base.UpdateAsync(route);
        }

        public async Task<IEnumerable<Route>> GetAllFullAsync()
        {
            return await _repository.GetAllFullAsync();
        }
    }
}