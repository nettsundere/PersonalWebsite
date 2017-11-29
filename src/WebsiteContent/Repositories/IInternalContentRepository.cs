using System;
using System.Collections.Generic;
using WebsiteContent.Models;

namespace WebsiteContent.Repositories
{
    /// <summary>
    /// Internal content repository.
    /// </summary>
    public interface IInternalContentRepository : IDisposable
    {
        void EnsureContentsRangeAvailable(IEnumerable<Content> contentsRange);
        void DeleteContentsByInternalCaptions(IEnumerable<string> internalCaptions);
    }
}
