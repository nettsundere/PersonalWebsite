using System.Threading.Tasks;
using PersonalWebsite.ViewModels.Content;
using WebsiteContent.Lib;

namespace PersonalWebsite.Services;

/// <summary>
/// Human-readable content retrieval service.
/// </summary>
public interface IHumanReadableContentRetrievalService 
{
    Task<PageViewModel> GetPageByHumanReadableNameAsync(LanguageDefinition languageDefinition, string urlName);
    Task<PageViewModel> GetPageByInternalCaptionAsync(LanguageDefinition languageDefinition, string internalCaption);
}