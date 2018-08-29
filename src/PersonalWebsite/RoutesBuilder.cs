using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using PersonalWebsite.Services;
using System;

namespace PersonalWebsite
{
    /// <summary>
    /// Website routes builder.
    /// </summary>
    internal class RoutesBuilder : IRoutesBuilder
    {
        private readonly ILanguageManipulationService _languageManipulationService;

        public RoutesBuilder(ILanguageManipulationService languageManipulationService)
        {
            _languageManipulationService = languageManipulationService ?? throw new ArgumentNullException(nameof(languageManipulationService));
        }

        /// <summary>
        /// Build routes.
        /// </summary>
        /// <param name="routes">Routes target.</param>
        public void Build(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: nameof(PersonalWebsite.Areas.Private),
                template: "{area:exists}/{controller}/{action}/{id?}",
                defaults: new { },
                constraints: new { area = nameof(PersonalWebsite.Areas.Private) }
            );

            var langRegex = _languageManipulationService.LanguageValidationRegexp;

            routes.MapRoute(
                name: "defaultWithLanguage",
                template: "{language}/{controller=Home}/{action=Index}",
                defaults: new { },
                constraints: new { language = langRegex }
            );

            routes.MapRoute(
                name: "defaultWithoutLanguage",
                template: "{controller=Home}/{action=Index}",
                defaults: new { language = string.Empty }
            );

            routes.MapRoute(
                name: "contentsWithLanguage",
                template: "{language}/{urlName}/{controller=Contents}/{action=Show}",
                defaults: new { },
                constraints: new { language = langRegex }
            );

            routes.MapRoute(
                name: "contentsWithoutLanguage",
                template: "{urlName}/{controller=Contents}/{action=Show}"
            );
        }
    }
}