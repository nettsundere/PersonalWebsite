using System.Collections.Generic;
using System.Threading.Tasks;
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
        Task EnsureContentListAvailableAsync(IReadOnlyList<Content> contentList);
    }
}
