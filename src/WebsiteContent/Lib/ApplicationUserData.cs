using System;

namespace WebsiteContent.Lib;

/// <summary>
/// Represents user data required to create a user.
/// </summary>
public class ApplicationUserData
{
    public string Name { get; }

    public string Password { get; }

    public string EMail { get; }

    public ApplicationUserData(string name, string email, string password)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException(nameof(email));
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException(nameof(password));
        }

        Name = name;
        Password = password;
        EMail = email;
    }
}