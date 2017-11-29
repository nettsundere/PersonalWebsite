using PersonalWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WebsiteContent.Models;
using WebsiteContent.Repositories;

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

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                 _context.Dispose();
                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// <see cref="InternalContentRepository"/> finalizer.
        /// </summary>
        ~InternalContentRepository()
        {
            Dispose();
        }


        /// <summary>
        /// Delete content having name in the list of <paramref name="internalCaptions"/>.
        /// </summary>
        /// <param name="internalCaptions">List of content captions to delete by.</param>
        public void DeleteContentsByInternalCaptions(IReadOnlyList<string> internalCaptions)
        {
            var contentsToRemove = from x in _context.Content
                                   where internalCaptions.Contains(x.InternalCaption)
                                   select x;

            _context.Content.RemoveRange(contentsToRemove);
            _context.SaveChanges();
        }

        /// <summary>
        /// Ensure that <paramref name="contentList"/> is available in the repository.
        /// </summary>
        /// <param name="contentList">Content list.</param>
        public void EnsureContentListAvailable(IReadOnlyList<Content> contentsRange)
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
