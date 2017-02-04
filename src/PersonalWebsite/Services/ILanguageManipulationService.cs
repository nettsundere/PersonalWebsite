using PersonalWebsite.Lib;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace PersonalWebsite.Services
{
    /// <summary>
    /// Language manipulation service.
    /// </summary>
    public interface ILanguageManipulationService
    {
        /// <summary>
        /// Supported cultures.
        /// </summary>
        ReadOnlyCollection<CultureInfo> SupportedCultures { get; }

        /// <summary>
        /// Language validation regular expression.
        /// </summary>
        string LanguageValidationRegexp { get; }

        /// <summary>
        /// Convert <see cref="LanguageDefinition"/> to its <see cref="string"/> representation.
        /// </summary>
        /// <param name="definition">language.</param>
        /// <returns>String representation.</returns>
        string LanguageDefinitionToLanguageRepresentation(LanguageDefinition definition);

        /// <summary>
        /// Convert <see cref="string"/> language representation to corresponding <see cref="LanguageDefinition"/> language.
        /// </summary>
        /// <param name="languageRepresentation">Language string representation.</param>
        /// <returns><see cref="LanguageDefinition"/> corresponding to <paramref name="languageRepresentation"/></returns>
        LanguageDefinition LanguageRepresentationToLanguageDefinition(string languageRepresentation);

        /// <summary>
        /// Find <see cref="CultureInfo"/> corresponding to <see cref="LanguageDefinition"/> language.
        /// </summary>
        /// <param name="languageDefinition">Language.</param>
        /// <returns><see cref="CultureInfo"/> corresponding to <paramref name="languageDefinition"/></returns>
        CultureInfo LanguageDefinitionToCultureInfo(LanguageDefinition languageDefinition);
    }
}
