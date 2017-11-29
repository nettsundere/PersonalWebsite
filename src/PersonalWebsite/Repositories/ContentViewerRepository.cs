using PersonalWebsite.Models;
using PersonalWebsite.ViewModels.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using WebsiteContent.Lib;
using WebsiteContent.Models;
using WebsiteContent.Repositories;
using WebsiteContent.Repositories.DTO;

namespace PersonalWebsite.Repositories
{
    /// <summary>
    /// Content repository.
    /// </summary>
    public class ContentViewerRepository : IContentViewerRepository
    {
        /// <summary>
        /// Data context.
        /// </summary>
        private readonly DataDbContext _dataDbContext;

        /// <summary>
        /// Disposing status.
        /// </summary>
        private bool _isDisposed = false;

        /// <summary>
        /// Create <see cref="ContentViewerRepository"/>.
        /// </summary>
        /// <param name="context">Data context.</param>
        public ContentViewerRepository(DataDbContext context)
        {
            _dataDbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Find translated content by language and internal caption.
        /// </summary>
        /// <param name="langDefinition">Required language.</param>
        /// <param name="internalCaption">Required internal caption.</param>
        /// <returns>Translated content representation.</returns>
        public ContentPublicViewData FindTranslatedContentByInternalCaption(LanguageDefinition langDefinition, string internalCaption)
        {
            GuardNotDisposed();

            var contentAndTranslation = (from translations in _dataDbContext.Translation
                                         join contents in _dataDbContext.Content
                                            on new { Id = translations.ContentId, Caption = internalCaption }
                                            equals new { Id = contents.Id, Caption = contents.InternalCaption }
                                         where translations.Version == langDefinition
                                               && translations.State == DataAvailabilityState.published
                                         select FillContentPublicViewData(contents.InternalCaption, translations, FindUrlNames(contents.Id))).FirstOrDefault();

            return contentAndTranslation;
        }

        /// <summary>
        /// Find translated content by language and url name.
        /// </summary>
        /// <param name="langDefinition">Required language.</param>
        /// <param name="urlName">Required Url name.</param>
        /// <returns>Translated content representation.</returns>
        public ContentPublicViewData FindTranslatedContentByUrlName(LanguageDefinition langDefinition, string urlName)
        {
            GuardNotDisposed();

            var lowerCaseUrlName = urlName.ToLowerInvariant();

            var contentAndTranslation = (from translation in _dataDbContext.Translation
                                         where translation.Version == langDefinition
                                              && translation.UrlName == lowerCaseUrlName
                                              && translation.State == DataAvailabilityState.published
                                         let content = translation.Content
                                         select FillContentPublicViewData(content.InternalCaption, translation, FindUrlNames(content.Id))).FirstOrDefault();

            return contentAndTranslation;
        }

        /// <summary>
        /// Get content links for a particular language.
        /// </summary>
        /// <param name="languageDefinition">Required language.</param>
        /// <param name="internalContentNames">List of required content names.</param>
        /// <returns>Data required to display all human-readable links to content pages depending on current language.</returns>
        public ContentPublicLinksData GetContentLinksPresentationData(LanguageDefinition languageDefinition, IList<string> internalContentNames)
        {
            GuardNotDisposed();

            var internalNamesToLinks = (from t in _dataDbContext.Translation
                                        where
                                          internalContentNames.Contains(t.Content.InternalCaption)
                                          && t.State == DataAvailabilityState.published
                                          && t.Version == languageDefinition
                                        select new
                                        {
                                            LinkUI = new ContentPublicLinkUI
                                            {
                                                LinkTitle = t.Title,
                                                UrlName = t.UrlName
                                            },
                                            InternalCaption = t.Content.InternalCaption
                                        }).ToDictionary(x => x.InternalCaption, x => x.LinkUI);

            return new ContentPublicLinksData()
            {
                InternalNamesToLinks = internalNamesToLinks
            };

        }

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~ContentViewerRepository()
        {
            Dispose();
        }

        /// <summary>
        /// Dispose the object.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _dataDbContext.Dispose();
                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        private void GuardNotDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(ContentViewerRepository));
            }
        }

        /// <summary>
        /// Fill <see cref="ContentPublicViewData"/> field.
        /// </summary>
        /// <param name="contentInternalCaption">Internal caption of a content.</param>
        /// <param name="translation">Content translation.</param>
        /// <param name="urlNames">Url names (for all content translations).</param>
        /// <returns>Data required to display a content.</returns>
        private ContentPublicViewData FillContentPublicViewData(string contentInternalCaption, Translation translation, IDictionary<LanguageDefinition, string> urlNames)
        {
            return new ContentPublicViewData
            {
                Title = translation.Title,
                Description = translation.Description,
                Markup = translation.ContentMarkup,
                UrlNames = urlNames,
                InternalCaption = contentInternalCaption
            };
        }

        /// <summary>
        /// Find all url names for a particular content.
        /// </summary>
        /// <param name="contentId">Content identifier.</param>
        /// <returns>All url names for a particular content.</returns>
        private Dictionary<LanguageDefinition, string> FindUrlNames(int contentId)
        {
            var urlNames = (
                from translations in _dataDbContext.Translation
                where
                    translations.ContentId == contentId
                    && translations.State == DataAvailabilityState.published
                select new { translations.Version, translations.UrlName }
            ).ToDictionary(z => z.Version, z => z.UrlName);

            return urlNames;
        }
    }
}
