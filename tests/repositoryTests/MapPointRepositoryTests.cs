using System;
using System.Threading.Tasks;
using domain.contexts;
using domain.repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace tests.repositoryTests
{
    public class MapPointRepositoryTests
    {
        private static SqliteConnection _connection;
        private static DbContextOptions _dbOptions;
        private static MainContext _context;
        private static MapPointRepository _mapPointRepository;

        public MapPointRepositoryTests()
        {
            string id = string.Format("{0}.db", Guid.NewGuid().ToString());
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = id,
                Mode = SqliteOpenMode.Memory,
                Cache = SqliteCacheMode.Shared
            };

            _connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
            _connection.Open();
            _connection.EnableExtensions();
            _dbOptions = new DbContextOptionsBuilder<MainContext>()
                        .UseSqlite(_connection)
                        .Options;
            _context = new MainContext(_dbOptions);
            Assert.True(_context.Database.EnsureCreated());
            _context.Database.Migrate();
            _context.ChangeTracker.AcceptAllChanges();
            _context.SaveChanges();
            _mapPointRepository = new MapPointRepository(_context);
        }

        [Fact]
        public async Task DatabaseIsEmpty()
        {
            var mapPoints = await _mapPointRepository.GetAllAsync();

            Assert.Empty(mapPoints);
        }
    }
}