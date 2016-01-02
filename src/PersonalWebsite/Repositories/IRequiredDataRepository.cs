using System.Collections.Generic;
using PersonalWebsite.Models;
using PersonalWebsite.Lib;

namespace PersonalWebsite.Repositories
{
    public interface IRequiredDataRepository
    {
        IList<Content> GetCriticalContent();

        ApplicationUserData GetInitialUserData();
    }
}