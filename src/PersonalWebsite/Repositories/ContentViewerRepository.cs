using PersonalWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebsiteContent.Lib;
using WebsiteContent.Models;
using WebsiteContent.Repositories;
using WebsiteContent.Repositories.DTO;

namespace PersonalWebsite.Repositories;

/// <summary>
/// Content repository.
/// </summary>
public class ContentViewerRepository : IContentViewerRepository
{
    /// <summary>
    /// Data context.
    /// </summary>
    private readonly DataDbContext _context;

    /// <summary>
    /// Create <see cref="ContentViewerRepository"/>.
    /// </summary>
    /// <param name="dataDbContext">Data context</param>
    public ContentViewerRepository(DataDbContext dataDbContext)
    {
        _context = dataDbContext ?? throw new ArgumentNullException(nameof(dataDbContext));
    }
        
    /// <summary>
    /// Find translated content by language and internal caption.
    /// </summary>
    /// <param name="langDefinition">Required language.</param>
    /// <param name="internalCaption">Required internal caption.</param>
    /// <returns>Translated content representation.</returns>
    public async Task<ContentPublicViewData> FindTranslatedContentByInternalCaptionAsync(LanguageDefinition langDefinition, string internalCaption)
    {
        var contentAndTranslation = await (from translation in _context.Translation
            join content in _context.Content
                on new {Id = translation.ContentId, Caption = internalCaption}
                equals new {Id = content.Id, Caption = content.InternalCaption}
            where translation.Version == langDefinition
                  && translation.State == DataAvailabilityState.published
            select new ContentAndTranslation(content, translation)).FirstOrDefaultAsync();
                
        return TryBuildContentPublicViewData(contentAndTranslation);
    }

    /// <summary>
    /// Find translated content by language and url name.
    /// </summary>
    /// <param name="langDefinition">Required language.</param>
    /// <param name="urlName">Required Url name.</param>
    /// <returns>Translated content representation.</returns>
    public async Task<ContentPublicViewData> FindTranslatedContentByUrlNameAsync(LanguageDefinition langDefinition, string urlName)
    {
        var lowerCaseUrlName = urlName.ToLowerInvariant();

        var contentAndTranslation = await (from translation in _context.Translation
            where translation.Version == langDefinition
                  && translation.UrlName == lowerCaseUrlName
                  && translation.State == DataAvailabilityState.published
            let content = translation.Content
            select new ContentAndTranslation(content, translation)).FirstOrDefaultAsync();

        return TryBuildContentPublicViewData(contentAndTranslation);
    }

    /// <summary>
    /// Get content links for a particular language.
    /// </summary>
    /// <param name="languageDefinition">Required language.</param>
    /// <param name="internalContentNames">List of required content names.</param>
    /// <returns>Data required to display all human-readable links to content pages depending on current language.</returns>
    public async Task<ContentPublicLinksData> GetContentLinksPresentationDataAsync(LanguageDefinition languageDefinition, IList<string> internalContentNames)
    {
        var internalNamesToLinks = await (from t in _context.Translation
            where
                internalContentNames.Contains(t.Content.InternalCaption)
                && t.State == DataAvailabilityState.published
                && t.Version == languageDefinition
            select new
            {
                LinkUI = new ContentPublicLinkUI(t.UrlName, t.Title), t.Content.InternalCaption
            }).ToDictionaryAsync(x => x.InternalCaption, x => x.LinkUI);
                
        return new ContentPublicLinksData(internalNamesToLinks);
    }

    /// <summary>
    /// Fill <see cref="ContentPublicViewData"/> field.
    /// </summary>
    /// <param name="contentInternalCaption">Internal caption of a content.</param>
    /// <param name="translation">Content translation.</param>
    /// <param name="urlNames">Url names (for all content translations).</param>
    /// <returns>Data required to display a content.</returns>
    private ContentPublicViewData FillContentPublicViewData(string contentInternalCaption, Translation translation, IDictionary<LanguageDefinition, string> urlNames)
    {
        return new ContentPublicViewData(
            translation.Title,
            translation.CustomHeaderMarkup,
            translation.ContentMarkup,
            translation.Description,
            contentInternalCaption,
            urlNames);
    }

    /// <summary>
    /// Find all url names for a particular content.
    /// </summary>
    /// <param name="contentId">Content identifier.</param>
    /// <returns>All url names for a particular content.</returns>
    private Dictionary<LanguageDefinition, string> FindUrlNames(int contentId)
    {
        var urlNames = (
            from translations in _context.Translation
            where
                translations.ContentId == contentId
                && translations.State == DataAvailabilityState.published
            select new { translations.Version, translations.UrlName }
        ).ToDictionary(z => z.Version, z => z.UrlName);

        return urlNames;
    }
        
    /// <summary>
    /// Try to build <see cref="ContentPublicViewData"/> using the <see cref="ContentAndTranslation"/> data.
    /// </summary>
    /// <param name="contentAndTranslation">Content and translation data.</param>
    /// <returns><see cref="ContentPublicViewData"/> data.</returns>
    private ContentPublicViewData TryBuildContentPublicViewData(ContentAndTranslation? contentAndTranslation)
    {
        if (contentAndTranslation is null)
        {
            throw new InvalidOperationException();
        }
            
        var content = contentAndTranslation.Content;
        var translation = contentAndTranslation.Translation;
                
        return FillContentPublicViewData(content.InternalCaption, translation, FindUrlNames(content.Id));
    }
}