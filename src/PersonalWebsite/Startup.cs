using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PersonalWebsite.Migrations;
using PersonalWebsite.Models;
using PersonalWebsite.Providers;
using PersonalWebsite.Repositories;
using PersonalWebsite.Services;
using WebsiteContent.Repositories;

namespace PersonalWebsite;

public class Startup
{
    public Startup(IHostEnvironment env)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

        builder.AddEnvironmentVariables();
        Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; set; }

    public void ConfigureServices(IServiceCollection services)
    {
        var poolSize = Configuration.GetValue<int>("PoolSize");
        services.AddDbContextPool<AuthDbContext>(ConfigureContext, poolSize);
        services.AddDbContextPool<DataDbContext>(ConfigureContext, poolSize);

        services.AddTransient<IDatabaseMigrationsRunner, DatabaseMigrationsRunner>();

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options => options.LoginPath = "/Private/Account/Login");

        services.AddMvc().AddViewLocalization();

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
    }

    /// <summary>
    /// Configure the context depending on a user's choice.
    /// </summary>
    /// <param name="services">Service provider</param>
    /// <param name="builder">SQL context builder</param>
    private void ConfigureContext(IServiceProvider services, DbContextOptionsBuilder builder)
    {
        var connectionString = Configuration.GetValue<string>("ConnectionStrings:Database");

        if (IsSqLiteEnabled())
        {
            builder.UseSqlite(connectionString);
        }
        else
        {
            builder.UseSqlServer(connectionString);
        }
    }

    /// <summary>
    /// Check if the SQLite is enabled. Enabling it overrides the option to use the MSSQL.
    /// </summary>
    /// <returns>A <see cref="bool" /> value indicating whether the SQLite is preferred.</returns>
    private bool IsSqLiteEnabled()
    {
        return Configuration["UseSQLite"].Contains("true", StringComparison.OrdinalIgnoreCase);
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