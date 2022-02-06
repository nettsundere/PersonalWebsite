using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonalWebsite.Models;
using PersonalWebsite.ViewModels.Manage;
using System;
using System.Threading.Tasks;

namespace PersonalWebsite.Areas.Private.Controllers;

/// <summary>
/// Management controller.
/// </summary>
[Authorize]
[Area(nameof(Private))]
public class ManageController : Controller
{
    /// <summary>
    /// Types of messages to show.
    /// </summary>
    public enum ManageMessageId
    {
        /// <summary>
        /// Password changed: success.
        /// </summary>
        ChangePasswordSuccess,

        /// <summary>
        /// Error.
        /// </summary>
        Error
    }

    /// <summary>
    /// User Manager.
    /// </summary>
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    /// SignIn Manager.
    /// </summary>
    private readonly SignInManager<ApplicationUser> _signInManager;

    /// <summary>
    /// Logger.
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// Create <see cref="ManageController"/>
    /// </summary>
    /// <param name="userManager">User manager.</param>
    /// <param name="signInManager">SignIn manager.</param>
    /// <param name="loggerFactory">Logger factory.</param>
    public ManageController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILoggerFactory loggerFactory)
    {
        if (loggerFactory == null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _logger = loggerFactory.CreateLogger<ManageController>();
    }

    /// <summary>
    /// Get Index, User management index page.
    /// </summary>
    /// <param name="message">Optional message to show.</param>
    /// <returns>User management index page.</returns>
    [HttpGet]
    public async Task<IActionResult> Index(ManageMessageId? message = null)
    {
        ViewData["StatusMessage"] =
            message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
            : message == ManageMessageId.Error ? "An error has occurred."
            : "";

        var user = await GetCurrentUserAsync();

        var logins = await _userManager.GetLoginsAsync(user); 
        var model = new IndexViewModel(logins, true);
        return View(model);
    }

    /// <summary>
    /// Get ChangePassword, get an interface to change a password.
    /// </summary>
    /// <returns>Interface to change a password.</returns>
    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    /// <summary>
    /// Post ChangePassword, password change attempt.
    /// </summary>
    /// <param name="model">Password change view model.</param>
    /// <returns>Redirect to management index or error.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var user = await GetCurrentUserAsync();
        if (user != null)
        {
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation(3, "User changed their password successfully.");
                return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }
        return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
    }

    /// <summary>
    /// Add errors.
    /// </summary>
    /// <param name="result">Identity result.</param>
    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    /// <summary>
    /// Get current user.
    /// </summary>
    /// <returns>Current application user.</returns>
    private Task<ApplicationUser> GetCurrentUserAsync()
    {
        return _userManager.GetUserAsync(HttpContext.User);
    }
}