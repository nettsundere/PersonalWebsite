using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using WebsiteContent.Repositories;
using WebsiteContent.Models;
using WebsiteContent.Lib;

namespace PersonalWebsite.Repositories;

/// <summary>
/// Provides access for required data generated in memory.
/// 
/// This is a substitution for the SEED data and should be used for any environment in order to generate
/// critical database records.
/// </summary>
public class RequiredDataRepository : IRequiredDataRepository
{
    /// <summary>
    /// Configuration.
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Create <see cref="RequiredDataRepository"/>.
    /// </summary>
    /// <param name="configuration">Configuration.</param>
    public RequiredDataRepository(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    /// Get mission-critical content.
    /// </summary>
    /// <returns>The list of exceptionally important content records.</returns>
    public IReadOnlyList<Content> GetCriticalContent()
    {
        var placeholderContentMarkup = "<h1>Header</h1>";
        var placeholderDescription = "Description";
            
        var contents = new [] {
            new Content
            {
                InternalCaption = PredefinedPages.Welcome.ToString(),
                Translations = new []
                {
                    new Translation { UrlName = "Welcome", Title = "Welcome", ContentMarkup = placeholderContentMarkup, Description = placeholderDescription, State = DataAvailabilityState.published, Version = LanguageDefinition.en_us },
                    new Translation { UrlName = "Willkommen", Title = "Willkommen", ContentMarkup = placeholderContentMarkup, Description = placeholderDescription, State = DataAvailabilityState.published, Version = LanguageDefinition.de_de },
                    new Translation { UrlName = "Privet",  Title = "Приветствую", ContentMarkup = placeholderContentMarkup, Description = placeholderDescription, State = DataAvailabilityState.published, Version = LanguageDefinition.ru_ru }
                }
            },
            new Content
            {
                InternalCaption = PredefinedPages.Contact.ToString(),
                Translations = new []
                {
                    new Translation { UrlName = "contact", Title = "Contact", ContentMarkup = placeholderContentMarkup, Description = placeholderDescription, State = DataAvailabilityState.published, Version = LanguageDefinition.en_us },
                    new Translation { UrlName = "kontakt", Title = "Kontakt", ContentMarkup = placeholderContentMarkup, Description = placeholderDescription, State = DataAvailabilityState.published, Version = LanguageDefinition.de_de },
                    new Translation { UrlName = "kontakty", Title = "Контакты", ContentMarkup = placeholderContentMarkup, Description = placeholderDescription, State = DataAvailabilityState.published, Version = LanguageDefinition.ru_ru }
                }
            },
            new Content
            {
                InternalCaption = PredefinedPages.Resume.ToString(),
                Translations = new []
                {
                    new Translation { UrlName = "resume", Title = "Resume", ContentMarkup = placeholderContentMarkup, Description = placeholderDescription, State = DataAvailabilityState.published, Version = LanguageDefinition.en_us },
                    new Translation { UrlName = "lebenslauf", Title = "Lebenslauf", ContentMarkup = placeholderContentMarkup, Description = placeholderDescription, State = DataAvailabilityState.published, Version = LanguageDefinition.de_de },
                    new Translation { UrlName = "rezyume",  Title = "Резюме", ContentMarkup = placeholderContentMarkup, Description = placeholderDescription, State = DataAvailabilityState.published, Version = LanguageDefinition.ru_ru }
                }
            }
        };

        return contents;
    }

    /// <summary>
    /// Get the <see cref="ApplicationUserData"/> of a default user.
    /// </summary>
    /// <returns>Default user data.</returns>
    public ApplicationUserData GetDefaultUserData()
    {
        var name = _configuration["CoreAccount:Name"];
        var email = _configuration["CoreAccount:Email"];
        var password = _configuration["CoreAccount:Password"];
        return new ApplicationUserData(name, email, password);
    }
}