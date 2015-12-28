using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersonalWebsite.Lib;

namespace PersonalWebsite.Services
{
    public class PageConfiguration : IPageConfiguration
    {
        public LanguageDefinition DefaultLanguage { get; } = LanguageDefinition.ru_ru;
        public string DefaultPageInternalCaption { get; } = PredefinedPages.Welcome.ToString();
    }
}
