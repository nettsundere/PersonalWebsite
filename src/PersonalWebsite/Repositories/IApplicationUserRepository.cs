using PersonalWebsite.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsite.Repositories
{
    public interface IApplicationUserRepository : IDisposable
    {
        void DeleteUserByEMail(string email);
        void EnsureUserAvailable(ApplicationUserData user);
    }
}
