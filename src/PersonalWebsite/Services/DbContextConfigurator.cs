using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PersonalWebsite.Services
{
    public class DbContextConfigurator
    {
        public IConfiguration Configuration { get; }

        public DbContextConfigurator(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Configure(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]);
        }
    }
}
