using PersonalWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonalWebsite.Repositories
{
    /// <summary>
    /// Internal content repository.
    /// </summary>
    public class InternalContentRepository : IInternalContentRepository
    {
        private bool _isDisposed = false;

        private readonly DataDbContext _context;

        /// <summary>
        /// Create <see cref="InternalContentRepository"/>.
        /// </summary>
        /// <param name="context">Data context.</param>
        public InternalContentRepository(DataDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                 _context.Dispose();
                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        ~InternalContentRepository()
        {
            Dispose();
        }

        public void DeleteContentsByInternalCaptions(IEnumerable<string> internalCaptions)
        {
            var contentsToRemove = from x in _context.Content
                                   where internalCaptions.Contains(x.InternalCaption)
                                   select x;

            _context.Content.RemoveRange(contentsToRemove);
            _context.SaveChanges();
        }

        public void EnsureContentsRangeAvailable(IEnumerable<Content> contentsRange)
        {
            var newContents = from x in contentsRange
                              let presentInternalCaptions = from y in _context.Content select y.InternalCaption
                              where !presentInternalCaptions.Contains(x.InternalCaption)
                              select x;
            _context.Content.AddRange(newContents);
            _context.SaveChanges();
        }
    }
}
