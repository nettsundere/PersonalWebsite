using PersonalWebsite.Lib;
using System;
using System.Collections.Generic;
using PersonalWebsite.ViewModels.Content;

namespace PersonalWebsite.Repositories
{
    /// <summary>
    /// Content repository.
    /// </summary>
    public interface IContentRepository : IDisposable
    {
        ContentViewModel FindTranslatedContentByUrlName(LanguageDefinition langDefinition, string urlName);
        ContentViewModel FindTranslatedContentByInternalCaption(LanguageDefinition langDefinition, string internalCaption);

        ContentLinksViewModel GetContentLinksPresentationData(LanguageDefinition languageDefinition, IList<string> internalContentNames);
    }
}
