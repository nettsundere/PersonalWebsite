using WebsiteContent.Lib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace PersonalWebsite.Services
{
    /// <summary>
    /// Language manipulation service.
    /// </summary>
    public class LanguageManipulationService : ILanguageManipulationService
    {
        /// <summary>
        /// Supported cultures list.
        /// </summary>
        public ReadOnlyCollection<CultureInfo> SupportedCultures { get; }

        private readonly IReadOnlyDictionary<LanguageDefinition, string> _languageDefinitionToRepresentations;
        private readonly IReadOnlyDictionary<string, LanguageDefinition> _languageRepresentationToLanguageDefinition;
        private readonly IReadOnlyDictionary<LanguageDefinition, CultureInfo> _languageDefinitionToCultureInfo;

        /// <summary>
        /// Language validation regular expression.
        /// </summary>
        public string LanguageValidationRegexp { get; }

        /// <summary>
        /// Create <see cref="LanguageManipulationService"/>.
        /// </summary>
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

            LanguageValidationRegexp = $"^({String.Join("|", _languageRepresentationToLanguageDefinition.Keys)})$";
        }
        /// <summary>
        /// Convert <see cref="LanguageDefinition"/> to its <see cref="string"/> representation.
        /// </summary>
        /// <param name="definition">language.</param>
        /// <returns>String representation of <paramref name="definition"/>.</returns>
        public string LanguageDefinitionToLanguageRepresentation(LanguageDefinition definition)
        {
            return _languageDefinitionToRepresentations[definition];
        }

        /// <summary>
        /// Convert <see cref="string"/> language representation to corresponding <see cref="LanguageDefinition"/> language.
        /// </summary>
        /// <param name="languageRepresentation">Language string representation.</param>
        /// <returns><see cref="LanguageDefinition"/> corresponding to <paramref name="languageRepresentation"/></returns>
        public LanguageDefinition LanguageRepresentationToLanguageDefinition(string languageRepresentation)
        {
            if (String.IsNullOrWhiteSpace(languageRepresentation))
            {
                throw new ArgumentException("Null or whitespace", nameof(languageRepresentation));
            }

            var lowerCaseRepresentation = languageRepresentation.ToLowerInvariant();

            return _languageRepresentationToLanguageDefinition[lowerCaseRepresentation];
        }

        /// <summary>
        /// Find <see cref="CultureInfo"/> corresponding to <see cref="LanguageDefinition"/> language.
        /// </summary>
        /// <param name="languageDefinition">Language.</param>
        /// <returns><see cref="CultureInfo"/> corresponding to <paramref name="languageDefinition"/></returns>
        public CultureInfo LanguageDefinitionToCultureInfo(LanguageDefinition languageDefinition)
        {
            return _languageDefinitionToCultureInfo[languageDefinition];
        }
    }
}
