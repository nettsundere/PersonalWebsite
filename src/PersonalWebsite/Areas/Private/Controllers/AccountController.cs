using Microsoft.Extensions.Logging;
using PersonalWebsite.Controllers;
using PersonalWebsite.Models;
using PersonalWebsite.ViewModels.Account;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PersonalWebsite.Areas.Private.Controllers;

/// <summary>
/// Account controller - private.
/// </summary>
[Authorize]
[Area(nameof(Private))]
public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger _logger;

    /// <summary>
    /// Create <see cref="AccountController"/>.
    /// </summary>
    /// <param name="signInManager">SignIn manager.</param>
    /// <param name="logger">Logger.</param>
    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        ILogger<AccountController> logger)
    {
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get Login.
    /// </summary>
    /// <returns>Login page.</returns>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    /// <summary>
    /// Post Login.
    /// </summary>
    /// <param name="loginViewModel">login view model.</param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation(1, "User logged in.");
                return RedirectToDashboard();
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning(2, "User account locked out.");
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(loginViewModel);
            }
        }

        // If we got this far, something failed, redisplay form
        return View(loginViewModel);
    }

    /// <summary>
    /// Post LogOff (logout).
    /// </summary>
    /// <returns>Redirect.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOff()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation(4, "User logged out.");
        return RedirectToAction(nameof(HomeController.Index), "Home", new { area = string.Empty });
    }

    /// <summary>
    /// Redirect to the dashboard.
    /// </summary>
    /// <returns>Redirect.</returns>
    private IActionResult RedirectToDashboard()
    {
        return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
    }
}