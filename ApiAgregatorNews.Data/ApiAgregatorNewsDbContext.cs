using ApiAgregatorNews.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace ApiAgregatorNews.Data
{
    public class ApiAgregatorNewsDbContext : DbContext
    {
        public DbSet<SourceRSS> SourcesRSS { get; set; }
        public DbSet<Item> Items { get; set; }
        public ApiAgregatorNewsDbContext(DbContextOptions options) : base(options)
        {

        }
    }

}
