using Microsoft.AspNetCore.Razor.TagHelpers;
using PersonalWebsite.Services;
using System;
using WebsiteContent.Lib;

namespace PersonalWebsite.Helpers
{
    /// <summary>
    /// Helper which prefixes href attributes of an A tag with language representation if required.
    /// Default language just adds / at the beginning.
    /// </summary>
    [HtmlTargetElement("a", Attributes = ListToRun)]
    public class UrlLanguagePrefixer : TagHelper
    {
        private readonly ILanguageManipulationService _languageManipulationService;
        private readonly IPageConfiguration _pageConfiguration;

        private const string ListToRun = "href,prefix-with-language";
        private const string LanguageAttributeName = "prefix-with-language";

        private const string HrefAttribute = "href";

        /// <summary>
        /// Language to prefix in href attribute.
        /// HREF attribute should contain relative url (no domain or schema parts).
        /// </summary>
        [HtmlAttributeName(LanguageAttributeName)]
        public LanguageDefinition LanguageToPrefix { get; set; }
        
        public UrlLanguagePrefixer(
            IPageConfiguration pageConfiguration,
            ILanguageManipulationService languageManipulationService)
        {
            _pageConfiguration = pageConfiguration ?? throw new ArgumentNullException(nameof(pageConfiguration));
            _languageManipulationService = languageManipulationService ?? throw new ArgumentNullException(nameof(languageManipulationService));
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
            var currentHref = output.Attributes[HrefAttribute].Value.ToString();

            var languageToPrefix = LanguageToPrefix;

            if (languageToPrefix == _pageConfiguration.DefaultLanguage)
            {
                if(string.IsNullOrEmpty(currentHref))
                {
                    output.Attributes.SetAttribute(HrefAttribute, "/");
                }
                else
                {
                    output.Attributes.SetAttribute(HrefAttribute, $"/{currentHref}");
                }
            }
            else
            {
                var languageRepresentation = _languageManipulationService.LanguageDefinitionToLanguageRepresentation(languageToPrefix);
                output.Attributes.SetAttribute(HrefAttribute, $"/{languageRepresentation}/{currentHref}");
            }
        }
    }
}
