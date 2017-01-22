using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonalWebsite.Controllers;
using PersonalWebsite.Models;
using PersonalWebsite.ViewModels.Account;
using System;
using System.Threading.Tasks;

namespace PersonalWebsite.Areas.Private.Controllers
{
    /// <summary>
    /// Account controller - private.
    /// </summary>
    [Authorize]
    [Area(nameof(Private))]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        /// <summary>
        /// Create <see cref="AccountController"/>.
        /// </summary>
        /// <param name="userManager">User manager.</param>
        /// <param name="signInManager">SignIn manager.</param>
        /// <param name="loggerFactory">Logger factory.</param>
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILoggerFactory loggerFactory)
        {
            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager));
            }

            if (signInManager == null)
            {
                throw new ArgumentNullException(nameof(signInManager));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
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
            return RedirectToAction(nameof(HomeController.Index), "Home", new { area = String.Empty });
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
        /// <returns>Current user.</returns>
        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        /// <summary>
        /// Redirect to the dashboard.
        /// </summary>
        /// <returns>Redirect.</returns>
        private IActionResult RedirectToDashboard()
        {
            return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
        }

        /// <summary>
        /// Redirect to local url or dashboard.
        /// </summary>
        /// <param name="returnUrl">Local url.</param>
        /// <returns>Redirect to <paramref name="returnUrl"/> or dashboard.</returns>
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToDashboard();
            }
        }
    }
}
