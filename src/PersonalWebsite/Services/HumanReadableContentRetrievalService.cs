using PersonalWebsite.Lib;
using PersonalWebsite.Repositories;
using PersonalWebsite.ViewModels.Content;
using System;

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
        private readonly IContentRepository _contentRepository;

        /// <summary>
        /// Page configuration.
        /// </summary>
        private readonly IPageConfiguration _pageConfiguration;

        /// <summary>
        /// Disposing status.
        /// </summary>
        private bool _isDisposed = false;

        /// <summary>
        /// Create <see cref="HumanReadableContentRetrievalService"/>.
        /// </summary>
        /// <param name="pageConfiguration">Page configuration.</param>
        /// <param name="contentRespository">Content repository.</param>
        public HumanReadableContentRetrievalService(
            IPageConfiguration pageConfiguration,
            IContentRepository contentRespository)
        {
            _contentRepository = contentRespository ?? throw new ArgumentNullException(nameof(contentRespository));
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
        /// Finalizer.
        /// </summary>
        ~HumanReadableContentRetrievalService() {
            Dispose();
        }

        /// <summary>
        /// Dispose the object.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _contentRepository.Dispose();
                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Throw if <see cref="HumanReadableContentRetrievalService"/> is disposed.
        /// </summary>
        private void GuardNotDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(HumanReadableContentRetrievalService));
            }
        }

        /// <summary>
        /// Get <see cref="PageViewModel"/> by language using content view model retrieval method.
        /// </summary>
        /// <param name="languageDefinition">Language.</param>
        /// <param name="contentViewModelMethod">Localized content retrieval method.</param>
        /// <returns></returns>
        private PageViewModel GetPageViewModelBy(LanguageDefinition languageDefinition, Func<ContentViewModel> contentViewModelMethod)
        {
            GuardNotDisposed();

            ContentViewModel contentVM;
            ContentLinksViewModel linksVM;
            using (_contentRepository)
            {
                contentVM = contentViewModelMethod();

                if (contentVM == null)
                {
                    return null; // No content available.
                }

                linksVM = _contentRepository.GetContentLinksPresentationData(languageDefinition,
                    Enum.GetNames(typeof(PredefinedPages))
                );
            }

            return new PageViewModel(_pageConfiguration, languageDefinition, contentVM, linksVM);
        }
    }
}
