using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Data Source=.\\sqlexpress;Initial Catalog=GuardPlanner;Integrated Security=SSPI;Persist Security Info=True;TrustServerCertificate=True");
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
