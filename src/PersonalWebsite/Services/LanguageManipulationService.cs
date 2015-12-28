using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersonalWebsite.Lib;
using System.Globalization;
using System.Threading;

namespace PersonalWebsite.Services
{
    public class LanguageManipulationService : ILanguageManipulationService
    {
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

            _languageDefinitionToCultureInfo = new Dictionary<LanguageDefinition, CultureInfo>
            {
				[LanguageDefinition.en_us] = new CultureInfo("en-us"),
				[LanguageDefinition.ru_ru] = new CultureInfo("ru-ru"),
				[LanguageDefinition.de_de] = new CultureInfo("de-de")
            };
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

        public void SetLanguage(LanguageDefinition languageDefinition)
        {
            var cultureToSet = _languageDefinitionToCultureInfo[languageDefinition];

            var currentThread = Thread.CurrentThread;

#if DNX451
			Thread.CurrentThread.CurrentCulture = cultureToSet;
			Thread.CurrentThread.CurrentUICulture = cultureToSet;
#elif DNXCORE50
            CultureInfo.CurrentCulture = cultureToSet;
            CultureInfo.CurrentUICulture = cultureToSet;
#endif
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
