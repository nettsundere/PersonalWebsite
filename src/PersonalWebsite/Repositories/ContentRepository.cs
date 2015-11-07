using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersonalWebsite.Models;
using PersonalWebsite.Lib;

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

        public Content FindByUrlName(LanguageDefinition langDefinition, string urlName)
        {
            GuardNotDisposed();

            var lowerCaseUrlName = urlName.ToLowerInvariant();
            _dataDbContext.Translations
                .Where(x => x.UrlName.ToLowerInvariant() == lowerCaseUrlName)
                .Where(x => x.Version == langDefinition)
                .First();

            return null;
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
