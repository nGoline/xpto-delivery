using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Xpto.Data.Context.Interfaces
{
    public interface IUnitOfWork<TContext>
        where TContext : DbContext
    {
        void BeginTransaction();
        Task SaveChangesAsync();
    }
}