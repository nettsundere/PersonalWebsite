using PersonalWebsite.Lib;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace PersonalWebsite.Services
{
    public interface ILanguageManipulationService
    {
        ReadOnlyCollection<CultureInfo> SupportedCultures { get; }

        string LanguageDefinitionToLanguageRepresentation(LanguageDefinition definition);

        LanguageDefinition LanguageRepresentationToLanguageDefinition(string LanguageRepresentation);

        CultureInfo LanguageDefinitionToCultureInfo(LanguageDefinition languageDefinition);

        string LanguageValidationRegexp();
    }
}
