using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using IPF_Beer_API.Data.Model;

namespace IPF_Beer_API.Data
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
            Database.OpenConnection();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Configuration.GetConnectionString("WebApiDatabase"));
        }

        public override void Dispose()
        {
            Database.CloseConnection();
            base.Dispose();
        }

        public DbSet<Beer> Beers { get; set; }

        public DbSet<Bar> Bars { get; set; }

        public DbSet<Brewery> Breweries { get; set; }
    }
}
