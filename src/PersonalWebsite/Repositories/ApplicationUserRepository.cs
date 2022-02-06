using Microsoft.AspNetCore.Identity;
using PersonalWebsite.Models;
using System;
using WebsiteContent.Lib;
using WebsiteContent.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsite.Repositories;

/// <summary>
/// Users repository.
/// </summary>
public class ApplicationUserRepository : IApplicationUserRepository
{
    /// <summary>
    /// Auth context.
    /// </summary>
    private readonly AuthDbContext _context;

    /// <summary>
    /// User manager.
    /// </summary>
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    /// Create <see cref="ApplicationUserRepository"/>.
    /// </summary>
    /// <param name="userManager">User manager.</param>
    /// <param name="authDbContext">Auth db context.</param>
    public ApplicationUserRepository(UserManager<ApplicationUser> userManager, AuthDbContext authDbContext)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _context = authDbContext ?? throw new ArgumentNullException(nameof(authDbContext));
    }

    /// <summary>
    /// Deletes a user by its EMail.
    /// </summary>
    /// <param name="email">EMail of a user to be deleted.</param>
    public async Task DeleteUserByEMailAsync(string email)
    {
        var usersToDelete =
            from x in _context.Users
            where x.Email == email
            select x;

        _context.Users.RemoveRange(usersToDelete);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Ensure <paramref name="user"/> exists.
    /// </summary>
    /// <param name="user">Required user.</param>
    public async Task EnsureUserAvailableAsync(ApplicationUserData user)
    {
        var sameEmailUsers = from x in _context.Users
            where x.Email == user.EMail
            select x;

        if (sameEmailUsers.Any()) return;
            
        var result = await _userManager.CreateAsync(new ApplicationUser { Email = user.EMail, UserName = user.Name }, user.Password);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Failed to ensure user {user.Name} is available");
        }
    }
}