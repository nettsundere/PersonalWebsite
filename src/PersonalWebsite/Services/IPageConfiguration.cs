using PersonalWebsite.Lib;
using WebsiteContent.Lib;

namespace PersonalWebsite.Services
{
    /// <summary>
    /// Page configuration.
    /// </summary>
    public interface IPageConfiguration
    {
        /// <summary>
        /// Default language.
        /// </summary>
        LanguageDefinition DefaultLanguage { get; }

        /// <summary>
        /// Internal caption of a default page.
        /// </summary>
        string DefaultPageInternalCaption { get; }
    }
}
