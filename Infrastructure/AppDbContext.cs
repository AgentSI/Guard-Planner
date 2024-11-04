using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Guard> Guards { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Vacation> Vacations { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Percent> Percents { get; set; }
        public DbSet<Instrument> Instrument { get; set; }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => ur.Id);

            modelBuilder.Entity<Guard>()
                .HasMany(g => g.Operations)
                .WithOne(o => o.Guard)
                .HasForeignKey(o => o.GuardId);

            modelBuilder.Entity<Operation>()
                .HasOne(o => o.Receipt)
                .WithOne(r => r.Operation)
                .HasForeignKey<Receipt>(r => r.OperationId);

            modelBuilder.Entity<Inventory>()
                .Property(i => i.Amount)
                .HasColumnType("decimal(18, 2)");
        }
    }
}
