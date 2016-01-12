using PersonalWebsite.Lib;
using System;

namespace PersonalWebsite.Repositories
{
    public interface IApplicationUserRepository : IDisposable
    {
        void DeleteUserByEMail(string email);
        void EnsureUserAvailable(ApplicationUserData user);
    }
}
