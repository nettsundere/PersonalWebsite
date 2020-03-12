using PersonalWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteContent.Models;
using WebsiteContent.Repositories;

namespace PersonalWebsite.Repositories
{
    /// <summary>
    /// Internal content repository.
    /// </summary>
    public class InternalContentRepository : IInternalContentRepository
    {
        /// <summary>
        /// Data context.
        /// </summary>
        private readonly DataDbContext _context;

        /// <summary>
        /// Create <see cref="InternalContentRepository"/>.
        /// </summary>
        /// <param name="dataDbContext">Data context.</param>
        public InternalContentRepository(DataDbContext dataDbContext)
        {
            _context = dataDbContext ?? throw new ArgumentNullException(nameof(dataDbContext));
        }

        /// <summary>
        /// Delete content having name in the list of <paramref name="internalCaptions"/>.
        /// </summary>
        /// <param name="internalCaptions">List of content captions to delete by.</param>
        public async Task DeleteContentsByInternalCaptionsAsync(IReadOnlyList<string> internalCaptions)
        {
            var contentsToRemove = from x in _context.Content
                where internalCaptions.Contains(x.InternalCaption)
                select x;

            _context.Content.RemoveRange(contentsToRemove);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Ensure that <paramref name="contentsRange"/> is available in the repository.
        /// </summary>
        /// <param name="contentsRange">Content range.</param>
        public async Task EnsureContentListAvailableAsync(IReadOnlyList<Content> contentsRange)
        {
            var newContents = from x in contentsRange
                let presentInternalCaptions = from y in _context.Content select y.InternalCaption
                where !presentInternalCaptions.Contains(x.InternalCaption)
                select x;
                
            _context.Content.AddRange(newContents);
            await _context.SaveChangesAsync();
        }
    }
}
