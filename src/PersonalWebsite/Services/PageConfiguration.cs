using PersonalWebsite.Lib;

namespace PersonalWebsite.Services
{
    public class PageConfiguration : IPageConfiguration
    {
        public LanguageDefinition DefaultLanguage { get; } = LanguageDefinition.en_us;
        public string DefaultPageInternalCaption { get; } = PredefinedPages.Welcome.ToString();
    }
}
