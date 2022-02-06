using Microsoft.AspNetCore.Identity;
using System;

namespace PersonalWebsite.Services;

/// <summary>
/// Identity configurator.
/// </summary>
public static class IdentityConfigurator
{
    /// <summary>
    /// Configures non-default identity options.
    /// </summary>
    /// <param name="options">Current identity options.</param>
    public static void Configure(IdentityOptions options)
    {
        if (options == null)
            throw new ArgumentNullException(nameof(options));

        var passwordOptions = options.Password;
        passwordOptions.RequireDigit = true;
        passwordOptions.RequireLowercase = true;
        passwordOptions.RequireUppercase = true;
        passwordOptions.RequireNonAlphanumeric = false;
        passwordOptions.RequiredLength = 7;
    }
}