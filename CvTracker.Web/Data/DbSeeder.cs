using System;
using System.Linq;
using CvTracker.Web.Models;
using CvTracker.Web.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CvTracker.Web.Data
{
    public static class DbSeeder
    {
        private const string MigrationId = "20260501061759_AddSchoolRegistrations";
        private const string EfProductVersion = "5.0.17";

        public static void Seed(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

            ApplyMigrations(context);

            if (context.Users.Any())
            {
                return;
            }

            context.Users.Add(new User
            {
                Name = "System Admin",
                Email = "admin@adasconnect.local",
                PasswordHash = passwordHasher.Hash("Admin@123"),
                Role = "Admin"
            });

            context.Users.Add(new User
            {
                Name = "System User",
                Email = "user@adasconnect.local",
                PasswordHash = passwordHasher.Hash("User@123"),
                Role = "User"
            });

            context.SaveChanges();
        }

        private static void ApplyMigrations(ApplicationDbContext context)
        {
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex) when (IsObjectAlreadyExists(ex))
            {
                EnsureLegacySchoolRegistrationsTable(context);
                EnsureMigrationHistoryRow(context);
            }
        }

        private static bool IsObjectAlreadyExists(Exception ex)
        {
            for (var e = ex; e != null; e = e.InnerException)
            {
                if (e is SqlException sql && sql.Number == 2714)
                {
                    return true;
                }
            }

            return false;
        }

        private static void EnsureLegacySchoolRegistrationsTable(ApplicationDbContext context)
        {
            context.Database.ExecuteSqlRaw(@"
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'SchoolRegistrations')
BEGIN
    CREATE TABLE [SchoolRegistrations] (
        [Id] int NOT NULL IDENTITY,
        [ChildName] nvarchar(120) NOT NULL,
        [Grade] int NOT NULL,
        [DateOfBirth] date NOT NULL,
        [PhotoPath] nvarchar(260) NULL,
        [FatherName] nvarchar(120) NOT NULL,
        [FatherProfession] nvarchar(120) NOT NULL,
        [FatherMobile] nvarchar(30) NOT NULL,
        [FatherEmail] nvarchar(150) NOT NULL,
        [FatherLandline] nvarchar(30) NULL,
        [MotherName] nvarchar(120) NOT NULL,
        [MotherProfession] nvarchar(120) NOT NULL,
        [MotherMobile] nvarchar(30) NOT NULL,
        [MotherEmail] nvarchar(150) NOT NULL,
        [MotherLandline] nvarchar(30) NULL,
        [AreaOfLiving] nvarchar(200) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_SchoolRegistrations] PRIMARY KEY ([Id])
    );
END
");
        }

        private static void EnsureMigrationHistoryRow(ApplicationDbContext context)
        {
            context.Database.ExecuteSqlRaw(@"
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'__EFMigrationsHistory')
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END
");

            context.Database.ExecuteSqlRaw($@"
IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'{MigrationId}')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'{MigrationId}', N'{EfProductVersion}');
END
");
        }
    }
}
