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
            if(languageManipulationService == null)
            {
                throw new ArgumentNullException(nameof(languageManipulationService));
            }

            _languageManipulationService = languageManipulationService;
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
                constraints: new { area = "private" }
            );
            
            routes.MapRoute(
                name: "defaultWithLanguage",
                template: "{language}/{controller=Home}/{action=Index}",
                defaults: new { },
                constraints: new { language = _languageManipulationService.LanguageValidationRegexp() }
            );

            routes.MapRoute(
                name: "defaultWithoutLanguage",
                template: "{controller=Home}/{action=Index}",
                defaults: new { language = String.Empty }
            );

            routes.MapRoute(
                name: "contentsWithLanguage",
                template: "{language}/{urlName}/{controller=Content}/{action=Show}",
                defaults: new { },
                constraints: new { language = _languageManipulationService.LanguageValidationRegexp() }
            );

            routes.MapRoute(
                name: "contentsWithoutLanguage",
                template: "{urlName}/{controller=Content}/{action=Show}"
            );
        }
    }
}