using System;
using System.Collections.Generic;
using WebsiteContent.Models;

namespace WebsiteContent.Repositories
{
    /// <summary>
    /// Internal content repository.
    /// </summary>
    public interface IInternalContentRepository 
    {
        /// <summary>
        /// Ensure that <paramref name="contentList"/> is available in the repository.
        /// </summary>
        /// <param name="contentList">Content list.</param>
        void EnsureContentListAvailable(IReadOnlyList<Content> contentList);

        /// <summary>
        /// Delete content having name in the list of <paramref name="internalCaptions"/>.
        /// </summary>
        /// <param name="internalCaptions">List of content captions to delete by.</param>
        void DeleteContentsByInternalCaptions(IReadOnlyList<string> internalCaptions);
    }
}
