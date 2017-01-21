using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace PersonalWebsite.Services
{
    /// <summary>
    /// Database context configurator.
    /// </summary>
    public class DbContextConfigurator
    {
        /// <summary>
        /// Configuration object.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Create <see cref="DbContextConfigurator"/>.
        /// </summary>
        /// <param name="configuration">Configuration object.</param>
        public DbContextConfigurator(IConfiguration configuration)
        {
            if(configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            Configuration = configuration;
        }

        /// <summary>
        /// Configure <see cref="DbContextOptionsBuilder"/> object using configuration.
        /// </summary>
        /// <param name="builder"><see cref="DbContextOptionsBuilder"/> object to configure.</param>
        public void Configure(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]);
        }
    }
}
