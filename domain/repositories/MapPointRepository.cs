using domain.contexts;
using domain.entities;

namespace domain.repositories
{
  public class MapPointRepository : RepositoryBase<MapPointEntity>
  {
    public MapPointRepository(MainContext context) : base(context)
    {
    }
  }
}