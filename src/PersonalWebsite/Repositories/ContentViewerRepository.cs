using PersonalWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
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
        /// Service provider.
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Create <see cref="ContentViewerRepository"/>.
        /// </summary>
        /// <param name="serviceProvider">Service provider.</param>
        public ContentViewerRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
        
        /// <summary>
        /// Find translated content by language and internal caption.
        /// </summary>
        /// <param name="langDefinition">Required language.</param>
        /// <param name="internalCaption">Required internal caption.</param>
        /// <returns>Translated content representation.</returns>
        public ContentPublicViewData FindTranslatedContentByInternalCaption(LanguageDefinition langDefinition, string internalCaption)
        {
            using (var dataDbContext = GetDataDbContext())
            {
                var contentAndTranslation = (from translation in dataDbContext.Translation
                    join content in dataDbContext.Content
                        on new { Id = translation.ContentId, Caption = internalCaption }
                        equals new { Id = content.Id, Caption = content.InternalCaption }
                    where translation.Version == langDefinition
                          && translation.State == DataAvailabilityState.published
                    select new ContentAndTranslation(content, translation)).FirstOrDefault();
                
                return BuildContentPublicViewData(contentAndTranslation);
            }
        }

        /// <summary>
        /// Find translated content by language and url name.
        /// </summary>
        /// <param name="langDefinition">Required language.</param>
        /// <param name="urlName">Required Url name.</param>
        /// <returns>Translated content representation.</returns>
        public ContentPublicViewData FindTranslatedContentByUrlName(LanguageDefinition langDefinition, string urlName)
        {
            var lowerCaseUrlName = urlName.ToLowerInvariant();

            using (var dataDbContext = GetDataDbContext())
            {
                var contentAndTranslation = (from translation in dataDbContext.Translation
                    where translation.Version == langDefinition
                          && translation.UrlName == lowerCaseUrlName
                          && translation.State == DataAvailabilityState.published
                    let content = translation.Content
                    select new ContentAndTranslation(content, translation)).FirstOrDefault();

                return BuildContentPublicViewData(contentAndTranslation);
            }
        }

        /// <summary>
        /// Get content links for a particular language.
        /// </summary>
        /// <param name="languageDefinition">Required language.</param>
        /// <param name="internalContentNames">List of required content names.</param>
        /// <returns>Data required to display all human-readable links to content pages depending on current language.</returns>
        public ContentPublicLinksData GetContentLinksPresentationData(LanguageDefinition languageDefinition, IList<string> internalContentNames)
        {
            using (var dataDbContext = GetDataDbContext())
            {
                var internalNamesToLinks = (from t in dataDbContext.Translation
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
                CustomHeaderMarkup = translation.CustomHeaderMarkup,
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
            using (var dataDbContext = GetDataDbContext())
            {
                var urlNames = (
                    from translations in dataDbContext.Translation
                    where
                        translations.ContentId == contentId
                        && translations.State == DataAvailabilityState.published
                    select new { translations.Version, translations.UrlName }
                ).ToDictionary(z => z.Version, z => z.UrlName);

                return urlNames;
            }
        }
        
        /// <summary>
        /// Build <see cref="ContentPublicViewData"/> using the <see cref="ContentAndTranslation"/> data.
        /// </summary>
        /// <param name="contentAndTranslation">Content and translation data.</param>
        /// <returns><see cref="ContentPublicViewData"/> data.</returns>
        private ContentPublicViewData BuildContentPublicViewData(ContentAndTranslation contentAndTranslation)
        {
            if (contentAndTranslation == default)
            {
                return default;
            }
            
            var content = contentAndTranslation.Content;
            var translation = contentAndTranslation.Translation;
                
            return FillContentPublicViewData(content.InternalCaption, translation, FindUrlNames(content.Id));
        }
        
        /// <summary>
        /// Get database context <see cref="DataDbContext"/>.
        /// </summary>
        /// <returns><see cref="DataDbContext"/></returns>
        private DataDbContext GetDataDbContext()
        {
            return _serviceProvider.GetRequiredService<DataDbContext>();
        }
    }
}
