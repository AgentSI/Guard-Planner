using Domain.Entities;
using Domain;

namespace Infrastructure.Initializer
{
    public static class DatabaseSeeder
    {
        public static void SeedDb(AppDbContext appDbContext)
        {
            AddMenuItems(appDbContext);
            AddUserRole(appDbContext);
            AddUsers(appDbContext);
            AddPercents(appDbContext);
            AddHours(appDbContext);
        }

        private static void AddMenuItems(AppDbContext appDbContext)
        {
            if (!appDbContext.MenuItems.Any())
            {
                var menuItems = new MenuItem[]
                {
                    new MenuItem { OriginalName = "Acasă", DisplayName = "Acasă", IsChecked = true, Href = "/", Icon = "Home"},

                    new MenuItem { OriginalName = "Generează Grafic", DisplayName = "Generează Grafic", IsChecked = true, Href = "/generate-guards", Icon = "AutoMode" },
                    new MenuItem { OriginalName = "Calendar Grafic", DisplayName = "Calendar Grafic", IsChecked = true, Href = "/guard-calendar", Icon = "CalendarMonth" },
                    new MenuItem { OriginalName = "Tabel Grafic", DisplayName = "Tabel Grafic", IsChecked = true, Href = "/guards", Icon = "DarkMode" },
                    new MenuItem { OriginalName = "Tabel Operații", DisplayName = "Tabel Operații", IsChecked = true, Href = "/operations", Icon = "MedicalServices" },

                    new MenuItem { OriginalName = "Tabel Concedii", DisplayName = "Tabel Concedii", IsChecked = true, Href = "/vacations", Icon = "WbSunny" },
                    new MenuItem { OriginalName = "Ore de lucru", DisplayName = "Ore de lucru", IsChecked = true, Href = "/hours", Icon = "HourglassFull" },
                    new MenuItem { OriginalName = "Tabel de pontaj", DisplayName = "Tabel de pontaj", IsChecked = true, Href = "/workers-hours", Icon = "AccessAlarm" },
                    new MenuItem { OriginalName = "Tabel Lucrători", DisplayName = "Tabel Lucrători", IsChecked = true, Href = "/workers", Icon = "Work" },
                    new MenuItem { OriginalName = "Procent gardă", DisplayName = "Procent gardă", IsChecked = true, Href = "/percents", Icon = "Percent" },

                    //new MenuItem { OriginalName = "Tabel Roluri", DisplayName = "Tabel Roluri", IsChecked = true, Href = "/roles", Icon = "PermIdentity" },
                    //new MenuItem { OriginalName = "Tabel Utilizatori", DisplayName = "Tabel Utilizatori", IsChecked = true, Href = "/users", Icon = "PersonAdd" },
                    new MenuItem { OriginalName = "Editare Menu", DisplayName = "Editare Menu", IsChecked = true, Href = "/edit-nav-menu", Icon = "Edit" },
                    new MenuItem { OriginalName = "Profilul meu", DisplayName = "Profilul meu", IsChecked = true, Href = "/my-profile", Icon = "Person" }
                };

                appDbContext.MenuItems.AddRange(menuItems);
                appDbContext.SaveChanges();
            }
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

        private static void AddHours(AppDbContext appDbContext)
        {
            if (!appDbContext.Hours.Any())
            {
                var hour1 = new Hour { Label = "8 ore", Value = 8 };
                var hour2 = new Hour { Label = "24 ore", Value = 24 };
                var hour3 = new Hour { Label = "Cn (concediu neplătit)", Value = 0 };
                var hour4 = new Hour { Label = "R (zile de repaus sau odihnă)", Value = 0 };
                var hour5 = new Hour { Label = "Sn (zile de sărbătoare nelucrătoare)", Value = 0 };
                var hour6 = new Hour { Label = "Cm (concediu medical)", Value = 0 };
                var hour7 = new Hour { Label = "Cc (concediu de maternitate)", Value = 0 };
                var hour8 = new Hour { Label = "C (concediu plătit)", Value = 0 };

                appDbContext.Hours.Add(hour1);
                appDbContext.Hours.Add(hour2);
                appDbContext.Hours.Add(hour3);
                appDbContext.Hours.Add(hour4);
                appDbContext.Hours.Add(hour5);
                appDbContext.Hours.Add(hour6);
                appDbContext.Hours.Add(hour7);
                appDbContext.Hours.Add(hour8);
                appDbContext.SaveChanges();
            }
        }
    }
}
