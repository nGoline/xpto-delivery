using System;
using System.Threading.Tasks;
using domain.contexts;
using domain.entities;
using Microsoft.EntityFrameworkCore;

namespace domain.repositories
{
    public class MapPointRepository : RepositoryBase<MapPointEntity>
    {
        public MapPointRepository(MainContext context) : base(context)
        {
        }
    }
}