using Microsoft.AspNet.Razor.TagHelpers;
using PersonalWebsite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersonalWebsite.Lib;

namespace PersonalWebsite.Helpers
{
    /// <summary>
    /// Helper which prefixes href attributes of an A tag with language representation if required.
    /// Default language just adds / at the beginning.
    /// </summary>
    [HtmlTargetElement("a", Attributes = listToRun)]
    public class UrlLanguagePrefixer : TagHelper
    {
        private readonly ILanguageManipulationService _languageManipulationService;
        private readonly IPageConfiguration _pageConfiguration;

        private const string listToRun = "href,prefix-with-language";
        private const string languageAttributeName = "prefix-with-language";

        /// <summary>
        /// Language to prefix in href attribute.
        /// HREF attribute should contain relative url (no domain or schema parts).
        /// </summary>
        [HtmlAttributeName(languageAttributeName)]
        public LanguageDefinition LanguageToPrefix { get; set; }
        
        public UrlLanguagePrefixer(
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

            _pageConfiguration = pageConfiguration;
            _languageManipulationService = languageManipulationService;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ProcessInternal(context, output);
            base.Process(context, output);
        }

        private void ProcessInternal(TagHelperContext context, TagHelperOutput output)
        {
            var currentHref = output.Attributes["href"].Value.ToString();
            if(currentHref.Length > 0)
            {
                currentHref += "/";
            }

            var languageToPrefix = LanguageToPrefix;

            if (languageToPrefix == _pageConfiguration.DefaultLanguage)
            {
                output.Attributes["href"].Value = $"/{currentHref}";
            }
            else
            {
                var languageRepresentation = _languageManipulationService.LanguageDefinitionToLanguageRepresentation(languageToPrefix);
                output.Attributes["href"].Value = $"/{languageRepresentation}/{currentHref}";
            }
        }
    }
}
