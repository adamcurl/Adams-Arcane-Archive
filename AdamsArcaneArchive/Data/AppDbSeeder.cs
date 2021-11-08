using AdamsArcaneArchive.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdamsArcaneArchive.Data
{
    public static class AppDbSeeder
    {
        public static IHost Migrate(this IHost webhost, ILogger logger)
        {
            using (IServiceScope scope = webhost.Services.GetService<IServiceScopeFactory>().CreateScope())
            using (AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>())
            using (var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>())
            using (var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>())
            {
                var environment = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                if (configuration.GetValue("Database:DeleteDatabase", false) == true)
                {
                    logger.Information("Deleting Database...");
                    context.Database.EnsureDeleted();
                }
                if (configuration.GetValue("Database:Migrate", false) == true)
                {
                    logger.Information("Migrating Database...");
                    context.Database.Migrate();
                }
                if (configuration.GetValue("Database:Seed", false) == true)
                {
                    logger.Information("Seeding Database...");
                    Seed(environment, context, userManager, roleManager, logger).Wait();
                }
            }
            return webhost;
        }

        public static async Task Seed(IHostEnvironment environment, AppDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ILogger logger)
        {
            // Add Roles
            if (!context.Roles.Any())
            {
                if (!await roleManager.RoleExistsAsync(AppRole.Administrator))
                    await roleManager.CreateAsync(new AppRole { Name = AppRole.Administrator });
                if (!await roleManager.RoleExistsAsync(AppRole.User))
                    await roleManager.CreateAsync(new AppRole { Name = AppRole.User });
            }

            // Add User
            if (!context.Users.Any())
            {
                // Admin Users
                List<string> adminRoles = new List<string> { AppRole.Administrator, AppRole.User };
                string devEmail = "dev@sodiumhalogen.com";
                string devPassword = "Password1!";

                var devUser = new AppUser
                {
                    UserName = devEmail,
                    Email = devEmail,
                    LockoutEnabled = false,
                    EmailConfirmed = true,
                    FirstName = "Sodium",
                    LastName = "Halogen",
                    PhoneNumber = "1112223333",
                };
                await userManager.CreateAsync(devUser, devPassword);
                await userManager.AddToRolesAsync(devUser, adminRoles);

                // Regular Users
                List<string> userRoles = new List<string> { AppRole.User };
                string userEmail = "user@sodiumhalogen.com";
                string userPassword = "Password1!";

                var user = new AppUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    LockoutEnabled = false,
                    EmailConfirmed = true,
                    FirstName = "Evan",
                    LastName = "Employee",
                    PhoneNumber = "7778889999",
                };
                await userManager.CreateAsync(user, userPassword);
                await userManager.AddToRolesAsync(user, userRoles);
            }

            await context.SaveChangesAsync();
        }
    }
}
