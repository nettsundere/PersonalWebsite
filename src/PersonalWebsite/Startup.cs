using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PersonalWebsite.Migrations;
using PersonalWebsite.Models;
using PersonalWebsite.Providers;
using PersonalWebsite.Repositories;
using PersonalWebsite.Services;
using WebsiteContent.Repositories;

namespace PersonalWebsite
{
    public class Startup
    {
        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables("Website");
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            var sqlContextConfigurator = new DbContextConfigurator(Configuration);
            services.AddEntityFrameworkSqlServer()
                .AddDbContextPool<AuthDbContext>(sqlContextConfigurator.Configure)
                .AddDbContextPool<DataDbContext>(sqlContextConfigurator.Configure);
            services.AddTransient<IDatabaseMigrationsRunner, DatabaseMigrationsRunner>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options => options.LoginPath = "/Private/Account/Login");

            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                    .AddViewLocalization();

            services.AddTransient<IContentViewerRepository, ContentViewerRepository>();
            services.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddTransient<IHumanReadableContentRetrievalService, HumanReadableContentRetrievalService>();
            services.AddTransient<IRequiredDataRepository, RequiredDataRepository>();
            services.AddTransient<IInternalContentRepository, InternalContentRepository>();

            services.AddTransient<IContentEditorRepository, ContentEditorRepository>();

            services.AddSingleton<IEndpointsBuilder, EndpointsBuilder>();
            services.AddSingleton<ILanguageManipulationService, LanguageManipulationService>();
            services.AddSingleton<IPageConfiguration, PageConfiguration>();
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddApplicationInsightsTelemetry();
        }

        public void Configure(
            IApplicationBuilder app,
            IHostEnvironment  env,
            IPageConfiguration pageConfiguration,
            ILanguageManipulationService languageManipulationService,
            IEndpointsBuilder endpointsBuilder)
        {
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            
            var defaultCulture = languageManipulationService
                                   .LanguageDefinitionToCultureInfo(
                                      pageConfiguration.DefaultLanguage
                                   );

            app.UseRequestLocalization(new RequestLocalizationOptions
                {
                    SupportedCultures = languageManipulationService.SupportedCultures,
                    SupportedUICultures = languageManipulationService.SupportedCultures,
                    RequestCultureProviders = new IRequestCultureProvider[] { new CustomUrlStringCultureProvider(languageManipulationService) },
                    DefaultRequestCulture = new RequestCulture(defaultCulture, defaultCulture)
                }
            );

            app.UseRouting(); 
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpointsBuilder.Build);
        }
    }
}
