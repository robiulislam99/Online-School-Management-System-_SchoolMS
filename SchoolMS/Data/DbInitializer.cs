using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolMS.Models;

namespace SchoolMS.Data
{
    public static class DbInitializer
    {
        /// <summary>
        /// Main method — call this from Program.cs
        /// Applies migrations and seeds default data
        /// </summary>
        public static async Task ApplyMigrationsAndSeed(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

            try
            {
                // STEP 1: Apply migrations first
                await ApplyMigrations(serviceProvider, logger);

                // STEP 2: Verify tables exist before seeding
                bool tablesExist = await CheckTablesExist(serviceProvider, logger);

                if (tablesExist)
                {
                    // STEP 3: Seed only if tables are ready
                    await SeedRolesAndAdmin(serviceProvider, logger);
                }
                else
                {
                    logger.LogError("❌ Tables do not exist. Please run: Update-Database");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during migration or seeding.");
                throw;
            }
        }

        // =============================================
        // STEP 1: Apply Migrations
        // =============================================
        private static async Task ApplyMigrations(
            IServiceProvider serviceProvider,
            ILogger logger)
        {
            var context = serviceProvider
                .GetRequiredService<ApplicationDbContext>();

            // Check if database can be connected
            bool canConnect = await context.Database.CanConnectAsync();
            if (!canConnect)
            {
                logger.LogError("❌ Cannot connect to database!");
                logger.LogError("Check your connection string in appsettings.json");
                throw new Exception("Cannot connect to database.");
            }

            logger.LogInformation("✅ Database connection successful.");

            // Check pending migrations
            var pendingMigrations = await context.Database
                .GetPendingMigrationsAsync();
            var pendingList = pendingMigrations.ToList();

            if (pendingList.Any())
            {
                logger.LogInformation(
                    "Applying {Count} pending migration(s)...",
                    pendingList.Count);

                foreach (var migration in pendingList)
                {
                    logger.LogInformation("  → {Migration}", migration);
                }

                // Apply all pending migrations
                await context.Database.MigrateAsync();
                logger.LogInformation("✅ All migrations applied successfully.");
            }
            else
            {
                logger.LogInformation("✅ No pending migrations.");
            }
        }

        // =============================================
        // STEP 2: Verify Tables Exist
        // =============================================
        private static async Task<bool> CheckTablesExist(
            IServiceProvider serviceProvider,
            ILogger logger)
        {
            var context = serviceProvider
                .GetRequiredService<ApplicationDbContext>();

            try
            {
                // Try to access a table — if it fails, tables don't exist
                var count = await context.Users.CountAsync();
                logger.LogInformation(
                    "✅ Tables verified. Current users: {Count}", count);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "❌ Tables not found: {Message}", ex.Message);
                logger.LogError(
                    "Run 'Update-Database' in Package Manager Console.");
                return false;
            }
        }

        // =============================================
        // STEP 3: Seed Roles and Admin User
        // =============================================
        private static async Task SeedRolesAndAdmin(
            IServiceProvider serviceProvider,
            ILogger logger)
        {
            var roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();
            var config = serviceProvider
                .GetRequiredService<IConfiguration>();

            // ---- Seed Roles ----
            string[] roles = { "Admin", "Teacher" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    logger.LogInformation("✅ Role created: {Role}", role);
                }
                else
                {
                    logger.LogInformation(
                        "✅ Role already exists: {Role}", role);
                }
            }

            // ---- Read credentials from appsettings.json ----
            var adminEmail = config["AdminSettings:Email"]
                                ?? "admin@school.com";
            var adminPassword = config["AdminSettings:Password"]
                                ?? "Admin@123";
            var adminName = config["AdminSettings:FullName"]
                                ?? "System Administrator";

            // ---- Seed Admin User ----
            var existingAdmin = await userManager
                .FindByEmailAsync(adminEmail);

            if (existingAdmin == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = adminName,
                    Role = "Admin",
                    EmailConfirmed = true
                };

                var result = await userManager
                    .CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    logger.LogInformation(
                        "✅ Admin user created: {Email}", adminEmail);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        logger.LogError(
                            "❌ Error creating admin: {Error}",
                            error.Description);
                    }
                }
            }
            else
            {
                logger.LogInformation(
                    "✅ Admin already exists: {Email}", adminEmail);
            }
        }
    }
}