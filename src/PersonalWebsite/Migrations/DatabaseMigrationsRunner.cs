using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PersonalWebsite.Models;

namespace PersonalWebsite.Migrations
{
    public class DatabaseMigrationsRunner : IDatabaseMigrationsRunner
    {   
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DatabaseMigrationsRunner> _runnerLogger;

        public DatabaseMigrationsRunner(IServiceProvider serviceProvider, ILogger<DatabaseMigrationsRunner> runnerLogger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _runnerLogger = runnerLogger ?? throw new ArgumentNullException(nameof(runnerLogger));
        }
        
        /// <summary>
        /// Run migrations using current Database connection.
        /// </summary>
        public void RunMigrations()
        {
            _runnerLogger.LogInformation("Running migrations");

            using (var authDbContext = _serviceProvider.GetRequiredService<AuthDbContext>())
            {
                MigrateDatabase(authDbContext.Database);   
            }
            
            using (var dataDbContext = _serviceProvider.GetRequiredService<AuthDbContext>())
            {
                MigrateDatabase(dataDbContext.Database);   
            }
        }

        /// <summary>
        /// Migrate the database.
        /// </summary>
        /// <param name="databaseFacade">Database facade.</param>
        private void MigrateDatabase(DatabaseFacade databaseFacade)
        {
            databaseFacade.Migrate();
        }
    }
}