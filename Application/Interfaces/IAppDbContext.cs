using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<User> Users { get; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Guard> Guards { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Vacation> Vacations { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Percent> Percents { get; set; }
        public DbSet<Instrument> Instrument { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
