using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonalWebsite.Models;
using PersonalWebsite.Providers;
using PersonalWebsite.Repositories;
using PersonalWebsite.Services;
using WebsiteContent.Repositories;

namespace PersonalWebsite
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var sqlContextConfigurator = new DbContextConfigurator(Configuration);
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<AuthDbContext>(sqlContextConfigurator.Configure, ServiceLifetime.Transient)
                .AddDbContext<DataDbContext>(sqlContextConfigurator.Configure, ServiceLifetime.Transient);

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options => options.LoginPath = "/Private/Account/Login");

            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    .AddViewLocalization();

            services.AddTransient<IContentViewerRepository, ContentViewerRepository>();
            services.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddTransient<IHumanReadableContentRetrievalService, HumanReadableContentRetrievalService>();
            services.AddTransient<IRequiredDataRepository, RequiredDataRepository>();
            services.AddTransient<IInternalContentRepository, InternalContentRepository>();

            services.AddTransient<IContentEditorRepository, ContentEditorRepository>();

            services.AddSingleton<IRoutesBuilder, RoutesBuilder>();
            services.AddSingleton<ILanguageManipulationService, LanguageManipulationService>();
            services.AddSingleton<IPageConfiguration, PageConfiguration>();
            services.AddSingleton<IConfiguration>(Configuration);
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            IPageConfiguration pageConfiguration,
            ILanguageManipulationService languageManipulationService,
            IRoutesBuilder routesBuilder)
        {
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

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

            app.UseMvc(routesBuilder.Build);
        }
    }
}
