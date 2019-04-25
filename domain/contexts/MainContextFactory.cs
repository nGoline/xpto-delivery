using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace domain.contexts
{
    public class MainContextFactory : IDesignTimeDbContextFactory<MainContext>, IDisposable
    {
        private DbConnection _connection;
        
        private DbContextOptions<MainContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<MainContext>()
                .UseInMemoryDatabase("Database 1")
                .Options;
        }

        public MainContext CreateDbContext(string[] args)
        {
            return CreateContext();
        }
        public MainContext CreateContext()
        {
            var options = CreateOptions();
            using (var context = new MainContext(options))
            {
                context.Database.EnsureCreated();
            }

            return new MainContext(CreateOptions());
        }

        public void Dispose()
        {
            if (_connection != null)
                _connection.Dispose();
        }
    }
}