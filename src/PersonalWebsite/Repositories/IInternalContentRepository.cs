using PersonalWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsite.Repositories
{
    public interface IInternalContentRepository : IDisposable
    {
        void EnsureContentsRangeAvailable(IEnumerable<Content> contentsRange);
        void DeleteContentsByInternalCaptions(IEnumerable<string> internalCaptions);
    }
}
