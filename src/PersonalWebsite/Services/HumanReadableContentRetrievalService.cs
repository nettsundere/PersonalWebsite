using PersonalWebsite.ViewModels.Content;
using System;
using System.Threading.Tasks;
using WebsiteContent.Lib;
using WebsiteContent.Repositories;
using WebsiteContent.Repositories.DTO;

namespace PersonalWebsite.Services;

/// <summary>
/// Human-readable content retrieval service.
/// </summary>
public class HumanReadableContentRetrievalService : IHumanReadableContentRetrievalService
{
    /// <summary>
    /// Content repository.
    /// </summary>
    private readonly IContentViewerRepository _contentRepository;

    /// <summary>
    /// Page configuration.
    /// </summary>
    private readonly IPageConfiguration _pageConfiguration;

    /// <summary>
    /// Create <see cref="HumanReadableContentRetrievalService"/>.
    /// </summary>
    /// <param name="pageConfiguration">Page configuration.</param>
    /// <param name="contentRepository">Content repository.</param>
    public HumanReadableContentRetrievalService(
        IPageConfiguration pageConfiguration,
        IContentViewerRepository contentRepository)
    {
        _contentRepository = contentRepository ?? throw new ArgumentNullException(nameof(contentRepository));
        _pageConfiguration = pageConfiguration ?? throw new ArgumentNullException(nameof(pageConfiguration));
    }

    /// <summary>
    /// Get translated page by its internal caption.
    /// </summary>
    /// <param name="languageDefinition">Required language definition.</param>
    /// <param name="internalCaption">Internal caption of a content.</param>
    /// <returns><typeparamref name="PageViewModel"/> instance</returns>
    public async Task<PageViewModel> GetPageByInternalCaptionAsync(LanguageDefinition languageDefinition, string internalCaption)
    {
        return await GetPageViewModelByAsync(languageDefinition, 
            () => _contentRepository.FindTranslatedContentByInternalCaptionAsync(languageDefinition, internalCaption));
    }

    /// <summary>
    /// Get translated page by its translated human-readable name.
    /// </summary>
    /// <param name="languageDefinition">Required language definition.</param>
    /// <param name="urlName">Human-readable name of a content.</param>
    /// <returns><typeparamref name="PageViewModel"/> instance</returns>
    public async Task<PageViewModel> GetPageByHumanReadableNameAsync(LanguageDefinition languageDefinition, string urlName)
    {
        return await GetPageViewModelByAsync(languageDefinition, 
            () => _contentRepository.FindTranslatedContentByUrlNameAsync(languageDefinition, urlName));
    }

    /// <summary>
    /// Get <see cref="PageViewModel"/> by language using content view model retrieval method.
    /// </summary>
    /// <param name="languageDefinition">Language.</param>
    /// <param name="contentViewModelMethod">Localized content retrieval method.</param>
    /// <returns>Page view model.</returns>
    private async Task<PageViewModel> GetPageViewModelByAsync(LanguageDefinition languageDefinition, Func<Task<ContentPublicViewData>> contentViewModelMethod)
    {
        var contentViewData = await contentViewModelMethod();
            
        var linksData = await _contentRepository.GetContentLinksPresentationDataAsync(languageDefinition,
            Enum.GetNames(typeof(PredefinedPages))
        );

        return new PageViewModel(_pageConfiguration, languageDefinition, contentViewData, linksData);
    }
}