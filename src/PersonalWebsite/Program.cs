using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PersonalWebsite.Migrations;
using PersonalWebsite.Services;

namespace PersonalWebsite
{
    public class Program
    {
        /// <summary>
        /// Website startup point.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope()) 
            {
                var migrationsRunner = scope.ServiceProvider.GetService<IDatabaseMigrationsRunner>();
                migrationsRunner.RunMigrations();
                
                var dataInitializer = new DataInitializer(scope.ServiceProvider);
                await dataInitializer.EnsureRequiredContentsAvailableAsync();
                await dataInitializer.EnsureInitialUserAvailableAsync();
            }
            
            host.Run();
        }

         private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(o => o.ListenAnyIP(8080));
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureLogging((context, logging) =>
                    {
                        logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                    
                        if (context.HostingEnvironment.IsDevelopment())
                        {
                            logging.AddDebug();
                        }

                        logging.AddConsole();
                    });
                });
    }
}
