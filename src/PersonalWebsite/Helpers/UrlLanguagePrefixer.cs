using Microsoft.AspNetCore.Razor.TagHelpers;
using PersonalWebsite.Services;
using System;
using WebsiteContent.Lib;

namespace PersonalWebsite.Helpers;

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
        var currentHref = output.Attributes[HrefAttribute]?.Value?.ToString();

        if (currentHref is null)
        {
            throw new InvalidOperationException("The href attribute is not defined");
        }

        var languageToPrefix = LanguageToPrefix;

        if (languageToPrefix == _pageConfiguration.DefaultLanguage)
        {
            output.Attributes.SetAttribute(HrefAttribute, GetCurrentOrRootHref(currentHref));
        }
        else
        {
            var languageRepresentation = _languageManipulationService.LanguageDefinitionToLanguageRepresentation(languageToPrefix);
            output.Attributes.SetAttribute(HrefAttribute, $"/{languageRepresentation}/{currentHref}");
        }
    }

    /// <summary>
    /// Get current href or default root href.
    /// </summary>
    /// <param name="currentHref">Possible current href.</param>
    /// <returns><paramref name="currentHref"/> href or default href.</returns>
    private static string GetCurrentOrRootHref(string currentHref)
    {
        return string.IsNullOrEmpty(currentHref) ? "/" : $"/{currentHref}";
    }
}