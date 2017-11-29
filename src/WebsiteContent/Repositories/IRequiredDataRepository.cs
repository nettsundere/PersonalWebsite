using System.Collections.Generic;
using WebsiteContent.Lib;
using WebsiteContent.Models;

namespace WebsiteContent.Repositories
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