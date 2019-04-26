using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Xpto.Data.Context.Interfaces
{
    public interface IContextFactory<TContext> : IDesignTimeDbContextFactory<TContext>, IDisposable
        where TContext : DbContext
    {
        TContext CreateDbContext();
    }
}