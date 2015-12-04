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
                    InternalCaption = "Contact",
                    Translations = new []
                    {
                        new Translation() { UrlName = "Contact", Title = "Contact", ContentMarkup = String.Empty, Description = String.Empty, State = DataAvailabilityState.published, Version = LanguageDefinition.en },
                        new Translation() { UrlName = "Kontakt", Title = "Kontakt", ContentMarkup = String.Empty, Description = String.Empty, State = DataAvailabilityState.published, Version = LanguageDefinition.de },
                        new Translation() { UrlName = "Kontakty", Title = "Контакты", ContentMarkup = String.Empty, Description = String.Empty, State = DataAvailabilityState.published, Version = LanguageDefinition.ru }
                    }
                },
                new Content()
                {
                    InternalCaption = "Resume",
                    Translations = new []
                    {
                        new Translation() { UrlName = "Resume", Title = "Resume", ContentMarkup = String.Empty, Description = String.Empty, State = DataAvailabilityState.published, Version = LanguageDefinition.en },
                        new Translation() { UrlName = "Lebenslauf", Title = "Lebenslauf", ContentMarkup = String.Empty, Description = String.Empty, State = DataAvailabilityState.published, Version = LanguageDefinition.de },
                        new Translation() { UrlName = "Rezyume",  Title = "Резюме", ContentMarkup = String.Empty, Description = String.Empty, State = DataAvailabilityState.published, Version = LanguageDefinition.ru }
                    }
                }
            };

            return contents;
        }
    }
}
