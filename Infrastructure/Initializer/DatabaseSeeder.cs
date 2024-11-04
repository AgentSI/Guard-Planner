using Domain.Entities;
using Domain;

namespace Infrastructure.Initializer
{
    public static class DatabaseSeeder
    {
        public static void SeedDb(AppDbContext appDbContext)
        {
            AddUserRole(appDbContext);
            AddUsers(appDbContext);
            AddPercents(appDbContext);
            AddWorkers(appDbContext);
        }

        private static void AddUserRole(AppDbContext appDbContext)
        {
            if (!appDbContext.UserRoles.Any())
            {
                var adminRole = new UserRole { RoleName = "Administrator" };
                var workerRole = new UserRole { RoleName = "Lucrător" };

                appDbContext.UserRoles.Add(adminRole);
                appDbContext.UserRoles.Add(workerRole);
                appDbContext.SaveChanges();
            }
        }

        private static void AddUsers(AppDbContext appDbContext)
        {
            string password = "Parola11a#";
            var passwordHash = Crypto.HashPassword(AuthorizationVariables.Salt + password);

            if (!appDbContext.Users.Any())
            {
                var adminRole = appDbContext.UserRoles.FirstOrDefault(r => r.RoleName == "Administrator");
                var workerRole = appDbContext.UserRoles.FirstOrDefault(r => r.RoleName == "Lucrător");

                var adminUser = new User
                {
                    PasswordHash = passwordHash,
                    Username = "Admin",
                    Email = "admin@mail.com",
                    UserRole = adminRole,
                    CreatedAt = DateTime.Now
                };

                var memberUser = new User
                {
                    PasswordHash = passwordHash,
                    Username = "Ana",
                    Email = "ana@mail.com",
                    UserRole = workerRole,
                    CreatedAt = DateTime.Now
                };

                appDbContext.Users.Add(adminUser);
                appDbContext.Users.Add(memberUser);
                appDbContext.SaveChanges();
            }
        }

        private static void AddPercents(AppDbContext appDbContext)
        {
            if (!appDbContext.Percents.Any())
            {
                var pefcent1 = new Percent { Value = 0.25 };
                var pefcent2 = new Percent { Value = 0.5 };
                var pefcent3 = new Percent { Value = 1 };

                appDbContext.Percents.Add(pefcent1);
                appDbContext.Percents.Add(pefcent2);
                appDbContext.Percents.Add(pefcent3);
                appDbContext.SaveChanges();
            }
        }

        private static void AddWorkers(AppDbContext appDbContext)
        {
            if (!appDbContext.Workers.Any())
            {
                var worker = new Worker { Name = "Lucrător", Email = "worker@mail.com", Available = true, IsGuard = true, Percent = 0.25 };
                var worker1 = new Worker { Name = "Ana", Email = "ana@mail.com", Available = true, IsGuard = true, Percent = 0.5 };
                var worker2 = new Worker { Name = "Ion", Email = "ion@mail.com", Available = true, IsGuard = true, Percent = 0.5 };
                var worker3 = new Worker { Name = "Inga", Email = "inga@mail.com", Available = true, IsGuard = true, Percent = 0.5 };
                var worker4 = new Worker { Name = "Lera", Email = "lera@mail.com", Available = true, IsGuard = true, Percent = 0.5 };
                var worker5 = new Worker { Name = "Nicu", Email = "nicu@mail.com", Available = true, IsGuard = true, Percent = 0.5 };
                var worker6 = new Worker { Name = "Dima", Email = "dima@mail.com", Available = true, IsGuard = true, Percent = 0.5 };
                var worker7 = new Worker { Name = "Ina", Email = "ina@mail.com", Available = true, IsGuard = true, Percent = 0.5 };
                var worker8 = new Worker { Name = "Olga", Email = "olga@mail.com", Available = true, IsGuard = true, Percent = 0.5 };

                appDbContext.Workers.Add(worker);
                appDbContext.Workers.Add(worker1);
                appDbContext.Workers.Add(worker2);
                appDbContext.Workers.Add(worker3);
                appDbContext.Workers.Add(worker4);
                appDbContext.Workers.Add(worker5);
                appDbContext.Workers.Add(worker6);
                appDbContext.Workers.Add(worker7);
                appDbContext.Workers.Add(worker8);
                appDbContext.SaveChanges();
            }
        }
    }
}
