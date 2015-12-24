using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersonalWebsite.Models;
using PersonalWebsite.Lib;
using PersonalWebsite.ViewModels.Content;
using Microsoft.Data.Entity;
using PersonalWebsite.Services;

namespace PersonalWebsite.Repositories
{
    public class ContentRepository : IContentRepository
    {
        private DataDbContext _dataDbContext;

        private ILanguageManipulationService _languageManipulationService;

        private bool _isDisposed = false;

        public ContentRepository(ILanguageManipulationService languageManipulationService, DataDbContext context)
        {
            if(context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if(languageManipulationService == null)
            {
                throw new ArgumentNullException(nameof(languageManipulationService));
            }

            _dataDbContext = context;
            _languageManipulationService = languageManipulationService;
        }

        public ContentViewModel FindTranslatedContentByInternalCaption(LanguageDefinition langDefinition, string internalCaption)
        {
            GuardNotDisposed();

            var contentAndTranslation = (from x in _dataDbContext.Translations
                                         join c in _dataDbContext.Contents 
                                            on new { Id = x.ContentId, Caption = internalCaption } 
                                            equals new { Id = c.Id, Caption = c.InternalCaption }
                                         where x.Version == langDefinition
                                               && x.State == DataAvailabilityState.published
                                         let urlNames = (from y in c.Translations
                                                         where y.State == DataAvailabilityState.published
                                                         select new { y.Version, y.UrlName }).ToDictionary(z => z.Version, z => z.UrlName)
                                         select FillContentViewModel(x, urlNames)).FirstOrDefault();

            return contentAndTranslation;
        }

        public ContentViewModel FindTranslatedContentByUrlName(LanguageDefinition langDefinition, string urlName)
        {
            GuardNotDisposed();

            var lowerCaseUrlName = urlName.ToLowerInvariant();
            var contentAndTranslation = (from x in _dataDbContext.Translations
                                         join c in _dataDbContext.Contents on x.ContentId equals c.Id
                                         where x.Version == langDefinition
                                              && x.UrlName == lowerCaseUrlName
                                              && x.State == DataAvailabilityState.published
                                         let urlNames = (from y in c.Translations
                                                        where y.State == DataAvailabilityState.published
                                                        select new { y.Version, y.UrlName }).ToDictionary(z => z.Version, z => z.UrlName)
                                         select FillContentViewModel(x, urlNames)).FirstOrDefault();

            return contentAndTranslation;
        }

        public ContentLinksViewModel GetContentLinksPresentationData(LanguageDefinition languageDefinition, IList<string> internalContentNames)
        {
            GuardNotDisposed();

            var internalNamesToLinkViewModels = (from t in _dataDbContext.Translations
                                              join c in _dataDbContext.Contents on t.ContentId equals c.Id
                                              where
                                                internalContentNames.Contains(c.InternalCaption)
                                                && t.State == DataAvailabilityState.published
                                                && t.Version == languageDefinition
                                              select new
                                              {
                                                  LinkUI = new LinkUI() {
                                                      LinkTitle = t.Title,
                                                      UrlName = t.UrlName
                                                  },
                                                  InternalCaption = c.InternalCaption
                                              }).ToDictionary(x => x.InternalCaption, x => x.LinkUI);

            return new ContentLinksViewModel(
                _languageManipulationService, 
                languageDefinition, 
                internalNamesToLinkViewModels);
        }

        ~ContentRepository()
        {
            Dispose();
        }

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
                throw new ObjectDisposedException(nameof(ContentRepository));
            }
        }

        private ContentViewModel FillContentViewModel(Translation translation, IDictionary<LanguageDefinition, string> urlNames)
        {
            return new ContentViewModel()
            {
                Title = translation.Title,
                Description = translation.Description,
                Markup = translation.ContentMarkup,
                UrlNames = urlNames
            };
        }
    }
}
