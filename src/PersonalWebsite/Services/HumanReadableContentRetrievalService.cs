using PersonalWebsite.ViewModels.Content;
using System;
using WebsiteContent.Lib;
using WebsiteContent.Repositories;
using WebsiteContent.Repositories.DTO;

namespace PersonalWebsite.Services
{
    /// <summary>
    /// Human-readable content retrieval service.
    /// </summary>
    public class HumanReadableContentRetrievalService : IHumanReadableContentRetrievalService
    {
        /// <summary>
        /// Content repository.
        /// </summary>
        private readonly IContentViewerRepository _contentRepository;

        /// <summary>
        /// Page configuration.
        /// </summary>
        private readonly IPageConfiguration _pageConfiguration;

        /// <summary>
        /// Create <see cref="HumanReadableContentRetrievalService"/>.
        /// </summary>
        /// <param name="pageConfiguration">Page configuration.</param>
        /// <param name="contentRepository">Content repository.</param>
        public HumanReadableContentRetrievalService(
            IPageConfiguration pageConfiguration,
            IContentViewerRepository contentRepository)
        {
            _contentRepository = contentRepository ?? throw new ArgumentNullException(nameof(contentRepository));
            _pageConfiguration = pageConfiguration ?? throw new ArgumentNullException(nameof(pageConfiguration));
        }

        /// <summary>
        /// Get translated page by its internal caption.
        /// </summary>
        /// <param name="languageDefinition">Required language definition.</param>
        /// <param name="internalCaption">Internal caption of a content.</param>
        /// <returns><typeparamref name="PageViewModel"/> instance</returns>
        public PageViewModel GetPageByInternalCaption(LanguageDefinition languageDefinition, string internalCaption)
        {
            return GetPageViewModelBy(languageDefinition, () => _contentRepository.FindTranslatedContentByInternalCaption(languageDefinition, internalCaption));
        }

        /// <summary>
        /// Get translated page by its translated human-readable name.
        /// </summary>
        /// <param name="languageDefinition">Required language definition.</param>
        /// <param name="urlName">Human-readable name of a content.</param>
        /// <returns><typeparamref name="PageViewModel"/> instance</returns>
        public PageViewModel GetPageByHumanReadableName(LanguageDefinition languageDefinition, string urlName)
        {
            return GetPageViewModelBy(languageDefinition, () => _contentRepository.FindTranslatedContentByUrlName(languageDefinition, urlName));
        }

        /// <summary>
        /// Get <see cref="PageViewModel"/> by language using content view model retrieval method.
        /// </summary>
        /// <param name="languageDefinition">Language.</param>
        /// <param name="contentViewModelMethod">Localized content retrieval method.</param>
        /// <returns></returns>
        private PageViewModel GetPageViewModelBy(LanguageDefinition languageDefinition, Func<ContentPublicViewData> contentViewModelMethod)
        {
            var contentViewData = contentViewModelMethod();

            if (contentViewData == null)
            {
                return null; // No content available.
            }

            var linksData = _contentRepository.GetContentLinksPresentationData(languageDefinition,
                Enum.GetNames(typeof(PredefinedPages))
            );

            return new PageViewModel(_pageConfiguration, languageDefinition, contentViewData, linksData);
        }
    }
}
