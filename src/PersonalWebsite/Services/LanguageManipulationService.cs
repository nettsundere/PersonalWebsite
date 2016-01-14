using System;
using System.Collections.Generic;
using PersonalWebsite.Lib;
using System.Globalization;
using System.Threading;
using System.Collections.ObjectModel;

namespace PersonalWebsite.Services
{
    public class LanguageManipulationService : ILanguageManipulationService
    {
        public ReadOnlyCollection<CultureInfo> SupportedCultures { get; }

        private readonly Dictionary<LanguageDefinition, string> _languageDefinitionToRepresentations;
        private readonly Dictionary<string, LanguageDefinition> _languageRepresentationToLanguageDefinition;
        private readonly Dictionary<LanguageDefinition, CultureInfo> _languageDefinitionToCultureInfo;

        public LanguageManipulationService()
        {
            _languageDefinitionToRepresentations = new Dictionary<LanguageDefinition, string>
            {
                [LanguageDefinition.en_us] = "en-us",
                [LanguageDefinition.ru_ru] = "ru-ru",
                [LanguageDefinition.de_de] = "de-de"
            };

            _languageRepresentationToLanguageDefinition = new Dictionary<string, LanguageDefinition>
            {
				["en-us"] = LanguageDefinition.en_us,
				["ru-ru"] = LanguageDefinition.ru_ru,
				["de-de"] = LanguageDefinition.de_de
            };

            var enCulture = new CultureInfo("en-us");
            var ruCulture = new CultureInfo("ru-ru");
            var deCulture = new CultureInfo("de-de");
            _languageDefinitionToCultureInfo = new Dictionary<LanguageDefinition, CultureInfo>
            {
				[LanguageDefinition.en_us] = enCulture,
				[LanguageDefinition.ru_ru] = ruCulture,
				[LanguageDefinition.de_de] = deCulture 
            };

            SupportedCultures = new ReadOnlyCollection<CultureInfo>(
                new [] { enCulture, ruCulture, deCulture });
        }

        public string LanguageDefinitionToLanguageRepresentation(LanguageDefinition definition)
        {
            return _languageDefinitionToRepresentations[definition];
        }

        public LanguageDefinition LanguageRepresentationToLanguageDefinition(string languageRepresentation)
        {
            if (String.IsNullOrWhiteSpace(languageRepresentation))
            {
                throw new ArgumentException("Null or whitespace", nameof(languageRepresentation));
            }

            var lowerCaseRepresentation = languageRepresentation.ToLowerInvariant();

            return _languageRepresentationToLanguageDefinition[lowerCaseRepresentation];
        }

        public CultureInfo LanguageDefinitionToCultureInfo(LanguageDefinition languageDefinition)
        {
            return _languageDefinitionToCultureInfo[languageDefinition];
        }

        public string LanguageValidationRegexp()
        {
            return $"^{LanguagesRegexpVariablePart()}$";
        }

        private string LanguagesRegexpVariablePart()
        {
            return String.Join("|", _languageRepresentationToLanguageDefinition.Keys);
        }
    }
}
