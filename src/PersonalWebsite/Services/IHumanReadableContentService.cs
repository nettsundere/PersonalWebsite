using PersonalWebsite.ViewModels.Content;
using System;
using WebsiteContent.Lib;

namespace PersonalWebsite.Services
{
    /// <summary>
    /// Human-readable content retrieval service.
    /// </summary>
    public interface IHumanReadableContentRetrievalService 
    {
        PageViewModel GetPageByHumanReadableName(LanguageDefinition languageDefinition, string urlName);
        PageViewModel GetPageByInternalCaption(LanguageDefinition languageDefinition, string internalCaption);
    }
}
