using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using PersonalWebsite.Services;
using System;

namespace PersonalWebsite;

/// <summary>
/// Website endpoints builder.
/// </summary>
internal class EndpointsBuilder : IEndpointsBuilder
{
    private readonly ILanguageManipulationService _languageManipulationService;

    public EndpointsBuilder(ILanguageManipulationService languageManipulationService)
    {
        _languageManipulationService = languageManipulationService ?? throw new ArgumentNullException(nameof(languageManipulationService));
    }

    /// <summary>
    /// Build endpoints.
    /// </summary>
    /// <param name="endpoints">Endpoints builder.</param>
    public void Build(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapControllerRoute(
            name: nameof(PersonalWebsite.Areas.Private),
            pattern: "{area:exists}/{controller}/{action}/{id?}",
            defaults: new { },
            constraints: new { area = nameof(PersonalWebsite.Areas.Private) }
        );

        var langRegex = _languageManipulationService.LanguageValidationRegexp;

        endpoints.MapControllerRoute(
            name: "defaultWithLanguage",
            pattern: "{language}/{controller=Home}/{action=Index}",
            defaults: new { },
            constraints: new { language = langRegex }
        );

        endpoints.MapControllerRoute(
            name: "defaultWithoutLanguage",
            pattern: "{controller=Home}/{action=Index}",
            defaults: new { language = string.Empty }
        );

        endpoints.MapControllerRoute(
            name: "contentsWithLanguage",
            pattern: "{language}/{urlName}/{controller=Contents}/{action=Show}",
            defaults: new { },
            constraints: new { language = langRegex }
        );

        endpoints.MapControllerRoute(
            name: "contentsWithoutLanguage",
            pattern: "{urlName}/{controller=Contents}/{action=Show}"
        );
    }
}