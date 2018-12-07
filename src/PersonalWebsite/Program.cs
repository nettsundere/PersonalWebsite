using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PersonalWebsite.Services;

namespace PersonalWebsite
{
    public class Program
    {
        /// <summary>
        /// Website startup point.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope()) 
            {
                var dataInitializer = new DataInitializer(scope.ServiceProvider);
                dataInitializer.EnsureRequiredContentsAvailable();
                dataInitializer.EnsureInitialUserAvailable();
            }
            
            host.Run();
        }

        /// <summary>
        /// Enable DbContext discovery 
        /// (allows dotnet ef database update to build DB Contexts properly)
        /// </summary>
        /// <param name="args">Startup arguments.</param>
        /// <returns></returns>
        private static IWebHost BuildWebHost(string[] args)
        {
            return new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .ConfigureLogging((context, logging) =>
                {
                    logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                    
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        logging.AddDebug();
                    }

                    logging.AddConsole();
                })
                .Build();
        }
    }
}
