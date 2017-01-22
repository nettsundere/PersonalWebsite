using PersonalWebsite.Lib;
using System;

namespace PersonalWebsite.Repositories
{
    /// <summary>
    /// Application user repository.
    /// </summary>
    public interface IApplicationUserRepository : IDisposable
    {
        void DeleteUserByEMail(string email);
        void EnsureUserAvailable(ApplicationUserData user);
    }
}
