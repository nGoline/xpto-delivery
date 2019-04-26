using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xpto.Data.Context.Interfaces;

namespace Xpto.Data.Context
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IDisposable
        where TContext : DbContext
    {
        private readonly DbContext _dbContext;
        private bool _disposed;

        public UnitOfWork(IContextFactory<TContext> contextFactory)
        {
            _dbContext = contextFactory.CreateDbContext();            
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void BeginTransaction()
        {
            _disposed = false;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }
    }
}