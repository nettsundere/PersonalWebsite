using PersonalWebsite.Lib;

namespace PersonalWebsite.Services
{
    public interface ILanguageManipulationService
    {
        string LanguageDefinitionToLanguageRepresentation(LanguageDefinition definition);

        LanguageDefinition LanguageRepresentationToLanguageDefinition(string LanguageRepresentation);

        void SetLanguage(LanguageDefinition languageDefinition);

        string LanguageValidationRegexp();
    }
}
