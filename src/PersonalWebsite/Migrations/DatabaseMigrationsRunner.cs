using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalWebsite.Models;

namespace PersonalWebsite.Migrations
{
    public class DatabaseMigrationsRunner : IDatabaseMigrationsRunner
    {
        private readonly AuthDbContext _authDbContext;
        private readonly DataDbContext _dataDbContext; 
        private readonly ILogger<DatabaseMigrationsRunner> _runnerLogger;
        public DatabaseMigrationsRunner(AuthDbContext authDbContext, DataDbContext dataDbContext, ILogger<DatabaseMigrationsRunner> runnerLogger)
        {
            _authDbContext = authDbContext ?? throw new ArgumentNullException(nameof(authDbContext));
            _dataDbContext = dataDbContext ?? throw new ArgumentNullException(nameof(dataDbContext));
            _runnerLogger = runnerLogger ?? throw new ArgumentNullException(nameof(runnerLogger));
        }
        
        /// <summary>
        /// Run migrations using current Database connection.
        /// </summary>
        public void RunMigrations()
        {
            _runnerLogger.LogInformation("Running migrations");
            _authDbContext.Database.Migrate();   
            _dataDbContext.Database.Migrate();
        }
    }
}