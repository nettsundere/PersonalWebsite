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
        /// <summary>
        /// Get mission-critical content.
        /// </summary>
        /// <returns>The list of exceptionally important content records.</returns>
        IReadOnlyList<Content> GetCriticalContent();

        /// <summary>
        /// Get the <see cref="ApplicationUserData"/> of a default user.
        /// </summary>
        /// <returns>Default user data.</returns>
        ApplicationUserData GetDefaultUserData();
    }
}