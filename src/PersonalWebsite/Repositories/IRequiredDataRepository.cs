using System.Collections.Generic;
using PersonalWebsite.Models;
using PersonalWebsite.Lib;

namespace PersonalWebsite.Repositories
{
    /// <summary>
    /// Required data repository.
    /// </summary>
    public interface IRequiredDataRepository
    {
        IReadOnlyList<Content> GetCriticalContent();

        ApplicationUserData GetInitialUserData();
    }
}