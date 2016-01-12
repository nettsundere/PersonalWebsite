using PersonalWebsite.Lib;

namespace PersonalWebsite.Services
{
    public interface IPageConfiguration
    {
        LanguageDefinition DefaultLanguage { get; }
        string DefaultPageInternalCaption { get; }
    }
}
