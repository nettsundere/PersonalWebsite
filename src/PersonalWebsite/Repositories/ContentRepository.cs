using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersonalWebsite.Models;
using PersonalWebsite.Lib;
using PersonalWebsite.ViewModels.Content;
using Microsoft.Data.Entity;

namespace PersonalWebsite.Repositories
{
    public class ContentRepository : IContentRepository
    {
        public DataDbContext _dataDbContext;

        private bool _isDisposed = false;

        public ContentRepository(DataDbContext context)
        {
            _dataDbContext = context;
        }

        public ContentViewModel FindTranslatedContent(LanguageDefinition langDefinition, string urlName)
        {
            GuardNotDisposed();

            var lowerCaseUrlName = urlName.ToLowerInvariant();
            var contentAndTranslation = _dataDbContext.Translations
                .FromSql("SELECT * FROM dbo.Translation AS T WHERE T.Version=(@p0) AND T.UrlName=(@p1) AND T.State=(@p2)", 
                langDefinition, 
                lowerCaseUrlName, 
                DataAvailabilityState.published)
                .Select(x => new ContentViewModel()
                {
                    Title = x.Title,
                    Description = x.Description,
                    Markup = x.ContentMarkup,
                    UrlNames = (from y in x.Content.Translations
                                where y.State == DataAvailabilityState.published
                                select new { y.Version, y.UrlName })
                               .ToDictionary(z => z.Version, z => z.UrlName)
                })
                .FirstOrDefault();

            return contentAndTranslation;
        }

        ~ContentRepository()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _dataDbContext.Dispose();
                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void GuardNotDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(ContentRepository));
            }
        }
    }
}
