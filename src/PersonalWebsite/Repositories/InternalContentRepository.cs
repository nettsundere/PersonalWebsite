using PersonalWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
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
        /// Service provider.
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Create <see cref="InternalContentRepository"/>.
        /// </summary>
        /// <param name="provider">Service provider.</param>
        public InternalContentRepository(IServiceProvider provider)
        {
            _serviceProvider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        /// <summary>
        /// Delete content having name in the list of <paramref name="internalCaptions"/>.
        /// </summary>
        /// <param name="internalCaptions">List of content captions to delete by.</param>
        public void DeleteContentsByInternalCaptions(IReadOnlyList<string> internalCaptions)
        {
            using (var dataDbContext = GetDataDbContext())
            {
                var contentsToRemove = from x in dataDbContext.Content
                    where internalCaptions.Contains(x.InternalCaption)
                    select x;

                dataDbContext.Content.RemoveRange(contentsToRemove);
                dataDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Ensure that <paramref name="contentsRange"/> is available in the repository.
        /// </summary>
        /// <param name="contentsRange">Content range.</param>
        public void EnsureContentListAvailable(IReadOnlyList<Content> contentsRange)
        {
            using (var dataDbContext = GetDataDbContext())
            {
                var newContents = from x in contentsRange
                    let presentInternalCaptions = from y in dataDbContext.Content select y.InternalCaption
                    where !presentInternalCaptions.Contains(x.InternalCaption)
                    select x;
                
                dataDbContext.Content.AddRange(newContents);
                dataDbContext.SaveChanges();
            }
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
