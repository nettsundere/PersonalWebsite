using PersonalWebsite.Lib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

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
