using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace domain.contexts
{
    public class MainContextFactory : IDesignTimeDbContextFactory<MainContext>
    {
        public MainContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MainContext>();
            optionsBuilder.UseSqlite("DataSource=:memory:");

            return new MainContext(optionsBuilder.Options);
        }
    }
}