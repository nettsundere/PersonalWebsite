using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteContent.Lib;
using WebsiteContent.Repositories.DTO;

namespace WebsiteContent.Repositories
{
    /// <summary>
    /// Content viewer repository.
    /// </summary>
    public interface IContentViewerRepository 
    {
        /// <summary>
        /// Find translated content by language and url name.
        /// </summary>
        /// <param name="langDefinition">Required language.</param>
        /// <param name="urlName">Required Url name.</param>
        /// <returns>Translated content representation.</returns>
        Task<ContentPublicViewData> FindTranslatedContentByUrlNameAsync(LanguageDefinition langDefinition, string urlName);

        /// <summary>
        /// Find translated content by language and internal caption.
        /// </summary>
        /// <param name="langDefinition">Required language.</param>
        /// <param name="internalCaption">Required internal caption.</param>
        /// <returns>Translated content representation.</returns>
        Task<ContentPublicViewData> FindTranslatedContentByInternalCaptionAsync(LanguageDefinition langDefinition, string internalCaption);

        /// <summary>
        /// Get content links for a particular language.
        /// </summary>
        /// <param name="languageDefinition">Required language.</param>
        /// <param name="internalContentNames">List of required content names.</param>
        /// <returns>Data required to display all human-readable links to content pages depending on current language.</returns>
        Task<ContentPublicLinksData> GetContentLinksPresentationDataAsync(LanguageDefinition languageDefinition, IList<string> internalContentNames);
    }
}
