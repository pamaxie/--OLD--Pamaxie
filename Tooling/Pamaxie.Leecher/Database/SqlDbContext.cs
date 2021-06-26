using Microsoft.EntityFrameworkCore;

namespace Pamaxie.Leecher.Database
{
    public class SqlDbContext : DbContext
    {
        public DbSet<LabelData> Label { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Data Source=localhost;Initial Catalog=PamaxieFilter;User Id=sa;Password=pass; MultipleActiveResultSets=True");
        }
    }
}
