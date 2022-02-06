using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PersonalWebsite.ViewModels.Manage;

public class IndexViewModel
{
    public bool HasPassword { get; }

    public IList<UserLoginInfo> Logins { get; }

    public IndexViewModel(IList<UserLoginInfo> logins, bool hasPassword)
    {
        Logins = logins ?? throw new ArgumentNullException(nameof(logins));
        HasPassword = hasPassword;
    }
}