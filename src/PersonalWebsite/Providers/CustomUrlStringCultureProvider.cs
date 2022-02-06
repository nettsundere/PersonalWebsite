using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using PersonalWebsite.Services;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PersonalWebsite.Providers
{
    public class CustomUrlStringCultureProvider : RequestCultureProvider
    {
        private readonly ILanguageManipulationService _languageManipulationService;

        public CustomUrlStringCultureProvider(ILanguageManipulationService languageManipulationService)
        {
            _languageManipulationService = languageManipulationService ?? throw new ArgumentNullException(nameof(languageManipulationService));
        }

        public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if(httpContext is null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var path = httpContext.Request.Path.ToString();
            var languageParts = path.Split('/');

            if (languageParts.Length > 0)
            {
                var maybeLanguage = languageParts[1];

                if(!string.IsNullOrWhiteSpace(maybeLanguage))
                {
                    var languageValidation = _languageManipulationService.LanguageValidationRegexp;
                    if (Regex.IsMatch(maybeLanguage, languageValidation, RegexOptions.IgnoreCase))
                    {
                        var languageDefinition = _languageManipulationService.LanguageRepresentationToLanguageDefinition(maybeLanguage);
                        var cultureInfo = _languageManipulationService.LanguageDefinitionToCultureInfo(languageDefinition);

                        var result = new ProviderCultureResult(cultureInfo.Name);
                        return Task.FromResult(result)!;
                    }
                }
            }

            // No result determined
            return NullProviderCultureResult;
        }
    }
}
