using PersonalWebsite.Models;
using System;
using System.Collections.Generic;

namespace PersonalWebsite.Repositories
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
