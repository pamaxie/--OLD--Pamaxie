using Microsoft.EntityFrameworkCore;

namespace Pamaxie.Leecher.Database
{
    class SqlDbContext : DbContext
    {
        public DbSet<LabelData> Label { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder Options)
        {
            Options.UseSqlServer("Data Source=localhost;Initial Catalog=PamaxieFilter;User Id=sa;Password=pass; MultipleActiveResultSets=True");
        }
    }
}
