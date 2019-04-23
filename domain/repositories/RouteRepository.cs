using domain.contexts;
using domain.entities;

namespace domain.repositories
{
  public class RouteRepository : RepositoryBase<RouteEntity>
  {
    public RouteRepository(MainContext context) : base(context)
    {
    }
  }
}