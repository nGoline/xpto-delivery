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
        private bool _disposed = false;

        private DbContextOptions<MainContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<MainContext>()
                .UseSqlite(_connection).Options;
        }
        
        public MainContext CreateDbContext(string[] args)
        {
            return CreateContext();
        }
        public MainContext CreateContext()
        {
            if (_connection == null)
            {
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();

                var options = CreateOptions();
                using (var context = new MainContext(options))
                {
                    context.Database.EnsureCreated();
                }
            }

            return new MainContext(CreateOptions());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _connection.Dispose();
            }

            _disposed = true;
        }
    }
}