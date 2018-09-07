using Microsoft.AspNetCore.Identity;
using PersonalWebsite.Models;
using System;
using Microsoft.Extensions.DependencyInjection;
using WebsiteContent.Lib;
using WebsiteContent.Repositories;
using System.Linq;

namespace PersonalWebsite.Repositories
{
    /// <summary>
    /// Users repository.
    /// </summary>
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        /// <summary>
        /// User manager.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Service provider.
        /// </summary>
        private readonly IServiceProvider _serviceProvider;
        
        /// <summary>
        /// Create <see cref="ApplicationUserRepository"/>.
        /// </summary>
        /// <param name="userManager">User manager.</param>
        /// <param name="serviceProvider">Service provider.</param>
        public ApplicationUserRepository(UserManager<ApplicationUser> userManager, IServiceProvider serviceProvider)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Deletes a user by its EMail.
        /// </summary>
        /// <param name="email">EMail of a user to be deleted.</param>
        public void DeleteUserByEMail(string email)
        {
            using (var authDbContext = GetAuthDbContext())
            {
                var usersToDelete =
                    from x in authDbContext.Users
                    where x.Email == email
                    select x;

                authDbContext.Users.RemoveRange(usersToDelete);
                authDbContext.SaveChanges();  
            }
        }

        /// <summary>
        /// Ensure <paramref name="user"/> exists.
        /// </summary>
        /// <param name="user">Required user.</param>
        public void EnsureUserAvailable(ApplicationUserData user)
        {
            using (var authDbContext = GetAuthDbContext())
            {
                var sameEmailUsers = from x in authDbContext.Users
                    where x.Email.Equals(user.EMail, StringComparison.OrdinalIgnoreCase)
                    select x;

                if (!sameEmailUsers.Any())
                {
                    var result = _userManager.CreateAsync(new ApplicationUser { Email = user.EMail, UserName = user.Name }, user.Password).Result;
                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException($"Failed to ensure user {user.Name} is available");
                    }
                }
            }
        }
        
        /// <summary>
        /// Get database context <see cref="AuthDbContext"/>.
        /// </summary>
        /// <returns><see cref="AuthDbContext"/>.</returns>
        private AuthDbContext GetAuthDbContext()
        {
            return _serviceProvider.GetRequiredService<AuthDbContext>();
        }
    }
}
