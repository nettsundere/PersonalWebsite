using System.Collections.Generic;
using PersonalWebsite.Models;

namespace PersonalWebsite.Repositories
{
    public interface IRequiredDataRepository
    {
        IList<Content> GetCriticalContent();
    }
}