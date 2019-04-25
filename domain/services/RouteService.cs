using domain.entities;
using domain.repositories;

namespace domain.services
{
    public class RouteService : ServiceBase<RouteEntity>
    {
        public RouteService(RouteRepository repository)
            : base(repository)
        { }
    }
}