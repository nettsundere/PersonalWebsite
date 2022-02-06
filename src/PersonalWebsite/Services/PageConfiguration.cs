using WebsiteContent.Lib;

namespace PersonalWebsite.Services;

/// <summary>
/// Page configuration.
/// </summary>
public class PageConfiguration : IPageConfiguration
{
    /// <summary>
    /// Default language.
    /// </summary>
    public LanguageDefinition DefaultLanguage { get; } = LanguageDefinition.en_us;

    /// <summary>
    /// Internal caption of a default page.
    /// </summary>
    public string DefaultPageInternalCaption { get; } = PredefinedPages.Welcome.ToString();
}