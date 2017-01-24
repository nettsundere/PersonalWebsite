using Microsoft.AspNetCore.Identity;
using PersonalWebsite.Lib;
using PersonalWebsite.Models;
using System;
using System.Linq;

namespace PersonalWebsite.Repositories
{
    /// <summary>
    /// Users repository.
    /// </summary>
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        /// <summary>
        /// Auth DB context.
        /// </summary>
        private readonly AuthDbContext _authDbContext;

        /// <summary>
        /// Disposing status.
        /// </summary>
        private bool _isDisposed;

        /// <summary>
        /// User manager.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Create <see cref="ApplicationUserRepository"/>.
        /// </summary>
        /// <param name="userManager">User manager.</param>
        /// <param name="authDbContext">Auth DB context.</param>
        public ApplicationUserRepository(UserManager<ApplicationUser> userManager, AuthDbContext authDbContext)
        {
            if(userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager));
            }

            if (authDbContext == null)
            {
                throw new ArgumentNullException(nameof(authDbContext));
            }

            _userManager = userManager;
            _authDbContext = authDbContext;
        }

        /// <summary>
        /// Dispose the object.
        /// </summary>
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

        /// <summary>
        /// Ensure <paramref name="user"/> exists.
        /// </summary>
        /// <param name="user">Required user.</param>
        public void EnsureUserAvailable(ApplicationUserData user)
        {
            GuardNotDisposed();

            var sameEmailUsers = from x in _authDbContext.Users
                                 where x.Email.Equals(user.EMail, StringComparison.OrdinalIgnoreCase)
                                 select x;

            if (sameEmailUsers.Count() == 0)
            {
                var result = _userManager.CreateAsync(new ApplicationUser { Email = user.EMail, UserName = user.Name }, user.Password).Result;
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Failed to ensure user {user.Name} is available");
                }
            }

        }

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~ApplicationUserRepository()
        {
            Dispose();
        }

        /// <summary>
        /// Throw if <see cref="ApplicationUserRepository"/> is disposed.
        /// </summary>
        private void GuardNotDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(ApplicationUserRepository));
            }
        }
    }
}
