using PersonalWebsite.Lib;
using PersonalWebsite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsite.ViewModels.Content
{
    /// <summary>
    /// Represents data required to display all human-readable links to content pages depending on current language. 
    /// </summary>
    public class ContentLinksViewModel
    {
        private ILanguageManipulationService _languageManipulationService;

        private IDictionary<string, LinkUI> _internalNamesToLinksUI;

        private LanguageDefinition _linksLanguage;

        public ContentLinksViewModel(
            ILanguageManipulationService languageManipulationService,
            LanguageDefinition linksLanguage,
            IDictionary<string, LinkUI> internalNamesToLinksUI)
        {
            if(languageManipulationService == null)
            {
                throw new ArgumentNullException(nameof(languageManipulationService));
            }

            if(internalNamesToLinksUI == null)
            {
                throw new ArgumentNullException(nameof(internalNamesToLinksUI));
            }

            _languageManipulationService = languageManipulationService;
            _internalNamesToLinksUI = internalNamesToLinksUI;
            _linksLanguage = linksLanguage;
        }

        public string CaptionFor(string internalContentCaption)
        {
            return _internalNamesToLinksUI[internalContentCaption].LinkTitle;
        }

        public string UrlFor(string internalContentCaption)
        {
            var urlName = _internalNamesToLinksUI[internalContentCaption].UrlName;

            if (_linksLanguage != _languageManipulationService.DefaultLanguageDefinition)
            {
                var languageRepresentation = _languageManipulationService.LanguageDefinitionToLanguageRepresentation(_linksLanguage);

                return $"/{languageRepresentation}/{urlName}/";
            }
            else
            {
                return $"/{urlName}/";
            }
        }
    }
} 
