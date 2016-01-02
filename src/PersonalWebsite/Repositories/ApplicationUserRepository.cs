using PersonalWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersonalWebsite.Lib;
using Microsoft.AspNet.Identity;

namespace PersonalWebsite.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly AuthDbContext _authDbContext;

        private bool _isDisposed = false;

        private readonly UserManager<ApplicationUser> _userManager;

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _authDbContext.Dispose();
                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Deletes a user by its EMail.
        /// </summary>
        /// <param name="email">EMail of a user to be deleted.</param>
        public void DeleteUserByEMail(string email)
        {
            GuardNotDisposed();

            var usersToDelete =
                 from x in _authDbContext.Users
                 where x.Email == email
                 select x;

            _authDbContext.Users.RemoveRange(usersToDelete);
            _authDbContext.SaveChanges();
        }

        public void EnsureUserAvailable(ApplicationUserData user)
        {
            GuardNotDisposed();

            var sameEmailUsers = from x in _authDbContext.Users where 
                                 x.Email.Equals(user.EMail, StringComparison.OrdinalIgnoreCase)
                                 select x;

            if(sameEmailUsers.Count() == 0)
            {
                var result = _userManager.CreateAsync(new ApplicationUser { Email = user.EMail, UserName = user.Name }, user.Password).Result;
                if(!result.Succeeded)
                {
                    throw new InvalidOperationException($"Failed to ensure user {user.Name} is available");
                }
            }

        }

        ~ApplicationUserRepository()
        {
            Dispose();
        }

        public ApplicationUserRepository(UserManager<ApplicationUser> userManager, AuthDbContext authDbContext)
        {
            if(authDbContext == null)
            {
                throw new ArgumentNullException(nameof(authDbContext));
            }

            if(userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager));
            }

            _userManager = userManager;
            _authDbContext = authDbContext;
        }

        private void GuardNotDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(ContentRepository));
            }
        }

    }
}
