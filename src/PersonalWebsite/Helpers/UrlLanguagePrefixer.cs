using Microsoft.AspNetCore.Razor.TagHelpers;
using PersonalWebsite.Services;
using System;
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

        private const string hrefAttribute = "href";

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
            ProcessLanguagePrefix(output);
            base.Process(context, output);
        }

        /// <summary>
        /// Process language prefix.
        /// </summary>
        /// <param name="output">Tag output.</param>
        private void ProcessLanguagePrefix(TagHelperOutput output)
        {
            var currentHref = output.Attributes[hrefAttribute].Value.ToString();

            var languageToPrefix = LanguageToPrefix;

            if (languageToPrefix == _pageConfiguration.DefaultLanguage)
            {
                if(String.IsNullOrEmpty(currentHref))
                {
                    output.Attributes.SetAttribute(hrefAttribute, "/");
                }
                else
                {
                    output.Attributes.SetAttribute(hrefAttribute, $"/{currentHref}");
                }
            }
            else
            {
                var languageRepresentation = _languageManipulationService.LanguageDefinitionToLanguageRepresentation(languageToPrefix);
                output.Attributes.SetAttribute(hrefAttribute, $"/{languageRepresentation}/{currentHref}");
            }
        }
    }
}
