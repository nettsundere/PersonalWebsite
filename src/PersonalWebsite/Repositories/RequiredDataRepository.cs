using PersonalWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersonalWebsite.Lib;

namespace PersonalWebsite.Repositories
{
    /// <summary>
    /// Provides access for required data generated in memory.
    /// 
    /// This is a substitution for the SEED data and should be used for any environment in order to generate
    /// critical database records.
    /// </summary>
    public class RequiredDataRepository : IRequiredDataRepository
    {
        public DataDbContext _dataDbContext;

        public IList<Content> GetCriticalContent()
        {
            var contents = new [] {
                new Content()
                {
                    InternalCaption = PredefinedPages.Welcome.ToString(),
                    Translations = new []
                    {
                        new Translation() { UrlName = "Welcome", Title = "Welcome", ContentMarkup = String.Empty, Description = String.Empty, State = DataAvailabilityState.published, Version = LanguageDefinition.en_us },
                        new Translation() { UrlName = "Willkommen", Title = "Willkommen", ContentMarkup = String.Empty, Description = String.Empty, State = DataAvailabilityState.published, Version = LanguageDefinition.de_de },
                        new Translation() { UrlName = "Privet",  Title = "Приветствую", ContentMarkup = String.Empty, Description = String.Empty, State = DataAvailabilityState.published, Version = LanguageDefinition.ru_ru }
                    }
                },
                new Content()
                {
                    InternalCaption = PredefinedPages.Contact.ToString(),
                    Translations = new []
                    {
                        new Translation() { UrlName = "Contact", Title = "Contact", ContentMarkup = String.Empty, Description = String.Empty, State = DataAvailabilityState.published, Version = LanguageDefinition.en_us },
                        new Translation() { UrlName = "Kontakt", Title = "Kontakt", ContentMarkup = String.Empty, Description = String.Empty, State = DataAvailabilityState.published, Version = LanguageDefinition.de_de },
                        new Translation() { UrlName = "Kontakty", Title = "Контакты", ContentMarkup = String.Empty, Description = String.Empty, State = DataAvailabilityState.published, Version = LanguageDefinition.ru_ru }
                    }
                },
                new Content()
                {
                    InternalCaption = PredefinedPages.Resume.ToString(),
                    Translations = new []
                    {
                        new Translation() { UrlName = "Resume", Title = "Resume", ContentMarkup = String.Empty, Description = String.Empty, State = DataAvailabilityState.published, Version = LanguageDefinition.en_us },
                        new Translation() { UrlName = "Lebenslauf", Title = "Lebenslauf", ContentMarkup = String.Empty, Description = String.Empty, State = DataAvailabilityState.published, Version = LanguageDefinition.de_de },
                        new Translation() { UrlName = "Rezyume",  Title = "Резюме", ContentMarkup = String.Empty, Description = String.Empty, State = DataAvailabilityState.published, Version = LanguageDefinition.ru_ru }
                    }
                }
            };

            return contents;
        }
    }
}
