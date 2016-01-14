using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PersonalWebsite.Models;
using PersonalWebsite.Services;
using PersonalWebsite.Repositories;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNet.Localization;
using PersonalWebsite.Providers;

namespace PersonalWebsite
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            // Setup configuration sources.

            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This reads the configuration keys from the secret store.
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var sqlContextConfigurator = new DbContextConfigurator(Configuration);
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<AuthDbContext>(sqlContextConfigurator.Configure)
                .AddDbContext<DataDbContext>(sqlContextConfigurator.Configure);

            // Add Identity services to the services container.
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            // Add MVC services to the services container.
            services.AddMvc()
                    .AddViewLocalization();

            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
            // services.AddWebApiConventions();

            // Register application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddTransient<IContentRepository, ContentRepository>();
            services.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddTransient<IHumanReadableContentService, HumanReadableContentService>();
            services.AddTransient<IRequiredDataRepository, RequiredDataRepository>();
            services.AddTransient<IInternalContentRepository, InternalContentRepository>();

            services.AddTransient<IContentEditorRepository, ContentEditorRepository>();

            services.AddSingleton<ILanguageManipulationService, LanguageManipulationService>();
            services.AddSingleton<IPageConfiguration, PageConfiguration>();
            services.AddInstance<IConfiguration>(Configuration);
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IPageConfiguration pageConfiguration,
            ILanguageManipulationService languageManipulationService)
        {
            if(pageConfiguration == null)
            {
                throw new ArgumentNullException(nameof(pageConfiguration));
            }

            if(languageManipulationService == null)
            {
                throw new ArgumentNullException(nameof(languageManipulationService));
            }

            loggerFactory.MinimumLevel = LogLevel.Information;
            loggerFactory.AddConsole();

            // Configure the HTTP request pipeline.

            // Add the following to the request pipeline only in development environment.
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage(x => x.EnableAll());

                loggerFactory.AddDebug(LogLevel.Debug);
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // sends the request to the following path or controller action.
                app.UseExceptionHandler("/Home/Error");

                loggerFactory.AddDebug(LogLevel.Information);
            }

            // Add the platform handler to the request pipeline.
            app.UseIISPlatformHandler();

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Add cookie-based authentication to the request pipeline.
            app.UseIdentity();

            var defaultCulture = languageManipulationService
                                   .LanguageDefinitionToCultureInfo(
                                      pageConfiguration.DefaultLanguage
                                   );
            app.UseRequestLocalization(new RequestLocalizationOptions
                {
                    SupportedCultures = languageManipulationService.SupportedCultures,
                    SupportedUICultures = languageManipulationService.SupportedCultures,
                    RequestCultureProviders = new[] { new CustomUrlStringCultureProvider(languageManipulationService) }
                }, 
                new RequestCulture(defaultCulture, defaultCulture)
            );

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: nameof(PersonalWebsite.Areas.Private),
                    template: "{area}/{controller}/{action}/{id?}",
                    defaults: new { },
                    constraints: new { area = "private" });
                routes.MapRoute(
                    name: "defaultWithLanguage",
                    template: "{language}/{controller=Home}/{action=Index}",
                    defaults: new { },
                    constraints: new { language = languageManipulationService.LanguageValidationRegexp() }
                );
                routes.MapRoute(
                    name: "defaultWithoutLanguage",
                    template: "{controller=Home}/{action=Index}",
                    defaults: new { language=String.Empty }
                );

                routes.MapRoute(
                    name: "contentsWithLanguage",
                    template: "{language}/{urlName}/{controller=Contents}/{action=Show}",
                    defaults: new { },
                    constraints: new { language = languageManipulationService.LanguageValidationRegexp() }
                );
                routes.MapRoute(
                    name: "contentsWithoutLanguage",
                    template: "{urlName}/{controller=Contents}/{action=Show}"
                );
            });

            using (var dataInitializer = new DataInitializer(app.ApplicationServices))
            {
                if (env.IsDevelopment())
                {
                    dataInitializer.ClearRequiredContents();
                    dataInitializer.ClearInitialUser();
                }

                dataInitializer.EnsureRequiredContentsAvailable();
                dataInitializer.EnsureInitialUserAvaialble();
            }
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
