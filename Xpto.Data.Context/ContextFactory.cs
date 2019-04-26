using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Xpto.Data.Context;
using Xpto.Data.Context.Interfaces;

namespace Xpto.Data.Context
{
    public class ContextFactory<TContext> : IContextFactory<TContext>
        where TContext : DbContext
    {
        private DbConnection _connection;

        private DbContextOptions<XptoContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<XptoContext>()
                .UseInMemoryDatabase("Database 1")
                .Options;
        }

        TContext IContextFactory<TContext>.CreateDbContext()
        {
            var options = CreateOptions();
            using (var context = new XptoContext(options))
            {
                context.Database.EnsureCreated();
            }

            return (TContext)Activator.CreateInstance(typeof(TContext), CreateOptions());
        }

        TContext IDesignTimeDbContextFactory<TContext>.CreateDbContext(string[] args)
        {
            var options = CreateOptions();
            using (var context = new XptoContext(options))
            {
                context.Database.EnsureCreated();
            }

            return (TContext)Activator.CreateInstance(typeof(TContext), CreateOptions());
        }

        public void Dispose()
        {
            if (_connection != null)
                _connection.Dispose();
        }
    }
}