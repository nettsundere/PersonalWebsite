using PersonalWebsite.Lib;
using PersonalWebsite.ViewModels.Content;
using System;

namespace PersonalWebsite.Services
{
    /// <summary>
    /// Human-readable content retrieval service.
    /// </summary>
    public interface IHumanReadableContentService : IDisposable
    {
        PageViewModel GetPageByHumanReadableName(LanguageDefinition languageDefinition, string urlName);
        PageViewModel GetPageByInternalCaption(LanguageDefinition languageDefinition, string internalCaption);
    }
}
