using PersonalWebsite.Lib;
using System;
using System.Collections.Generic;

namespace PersonalWebsite.ViewModels.Content
{
    /// <summary>
    /// Represents data required to display all human-readable links to content pages depending on current language. 
    /// </summary>
    public class ContentLinksViewModel
    {
        private readonly IDictionary<string, LinkUI> _internalNamesToLinksUI;

        private readonly LanguageDefinition _linksLanguage;

        public ContentLinksViewModel(
            LanguageDefinition linksLanguage,
            IDictionary<string, LinkUI> internalNamesToLinksUI)
        {
            if(internalNamesToLinksUI == null)
            {
                throw new ArgumentNullException(nameof(internalNamesToLinksUI));
            }

            _internalNamesToLinksUI = internalNamesToLinksUI;
            _linksLanguage = linksLanguage;
        }

        public string CaptionFor(string internalContentCaption)
        {
            return _internalNamesToLinksUI[internalContentCaption].LinkTitle;
        }

        public string UrlNameFor(string internalContentCaption)
        {
            return _internalNamesToLinksUI[internalContentCaption].UrlName;
        }
    }
} 
