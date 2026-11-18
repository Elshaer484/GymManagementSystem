using GymManagment.DAL.Data.DataSeeding;
using GymManagment.DAL.Data.DbContexts;
using GymManagment.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace GymManagment.PL
{
    public static class ProgramExtenstion
    {
        public static async Task MigrateAndSeedData(this WebApplication app)
        {
            using var scoped = app.Services.CreateScope();

            var dbcontext = scoped.ServiceProvider.GetRequiredService<GymDbContext>();

            var logger = scoped.ServiceProvider.GetRequiredService<ILogger<Program>>();

            var RoleMan = scoped.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var UserMan = scoped.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var pendingMigration = await dbcontext.Database.GetPendingMigrationsAsync();

            if (pendingMigration.Any())
            {
                logger.LogInformation($"Applying {pendingMigration.Count()} Pending Migrations");
                await dbcontext.Database.MigrateAsync();
            }

            var seedFolder = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");



            await GymDataSeeding.SeedAsync(dbcontext, seedFolder, logger);

            await IdentityDataSeeding.IdentityRoleAsync(RoleMan, UserMan , logger);
        }
    }
}
