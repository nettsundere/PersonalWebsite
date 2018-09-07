using System;
using WebsiteContent.Lib;

namespace WebsiteContent.Repositories
{
    /// <summary>
    /// Application user repository.
    /// </summary>
    public interface IApplicationUserRepository 
    {
        /// <summary>
        /// Deletes a user by its EMail.
        /// </summary>
        /// <param name="email">EMail of a user to be deleted.</param>
        void DeleteUserByEMail(string email);

        /// <summary>
        /// Ensure the <paramref name="user"/> exists.
        /// </summary>
        /// <param name="user">User data.</param>
        void EnsureUserAvailable(ApplicationUserData user);
    }
}
