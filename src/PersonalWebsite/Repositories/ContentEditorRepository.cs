using Microsoft.EntityFrameworkCore;
using PersonalWebsite.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.IIS.Core;
using PersonalWebsite.Lib.Extensions;
using WebsiteContent.Lib;
using WebsiteContent.Models;
using WebsiteContent.Repositories;
using WebsiteContent.Repositories.DTO;

namespace PersonalWebsite.Repositories;

/// <summary>
/// Content editor repository.
/// </summary>
public class ContentEditorRepository : IContentEditorRepository
{
    /// <summary>
    /// Data context.
    /// </summary>
    private readonly DataDbContext _context;
        
    /// <summary>
    /// Create <see cref="ContentEditorRepository"/>.
    /// </summary>
    /// <param name="dataDbContext">Data context.</param>
    public ContentEditorRepository(DataDbContext dataDbContext)
    {
        _context = dataDbContext ?? throw new ArgumentNullException(nameof(dataDbContext));
    }
        
    /// <summary>
    /// Create <see cref="ContentPrivateEditData"/> representation in DB.
    /// </summary>
    /// <param name="contentEditViewModel">Content representation to store in DB.</param>
    /// <returns>Updated content representation.</returns>
    public async Task<ContentPrivateEditData> CreateAsync(ContentPrivateEditData contentEditViewModel)
    {
        if (contentEditViewModel is null)
        {
            throw new ArgumentNullException(nameof(contentEditViewModel));
        }

        var content = _context.Content.Add(new Content
        {
            InternalCaption = contentEditViewModel.InternalCaption
        });

        await _context.SaveChangesAsync();

        var entity = content.Entity;

        return new ContentPrivateEditData(entity);
    }

    /// <summary>
    /// Read a content.
    /// </summary>
    /// <param name="contentId">Id of a content to read.</param>
    /// <returns>Content representation.</returns>
    public async Task<ContentPrivateEditData> ReadAsync(int contentId)
    {
        var content = await _context.Content.Include(c => c.Translations).FirstAsync(x => x.Id == contentId);
        var data = new ContentPrivateEditData(content);

        // Build missing translations
        var missingTranslations = from x in Enum.GetValues(typeof(LanguageDefinition)).Cast<LanguageDefinition>()
            where !data.Translations.Select(t => t.Version).Contains(x)
            select new TranslationPrivateEditData(new Translation()) { ContentId = data.Id, Version = x };

        data.Translations = data.Translations.Concat(missingTranslations).ToList();

        return data;
    }

    /// <summary>
    /// Read content list.
    /// </summary>
    /// <returns>List of content representations.</returns>
    public async Task<ContentPrivateEditListData> ReadListAsync()
    {
        var contentsQuery = from x in _context.Content
            orderby x.Id
            select new ContentPrivateLinksData(x.Id, x.InternalCaption);

        return new ContentPrivateEditListData(await contentsQuery.ToListAsync());
    }

    /// <summary>
    /// Update a content.
    /// </summary>
    /// <param name="data">Data to update with.</param>
    /// <returns>Updated content.</returns>
    public async Task<ContentPrivateEditData> UpdateAsync(ContentPrivateEditData data)
    {
        if (data is null)
        {
            throw new ArgumentNullException(nameof(data));
        }
            
        var content = await _context.Content.Include(c => c.Translations)
            .SingleAsync(x => x.Id == data.Id);

        var updateTime = DateTime.UtcNow;

        content.InternalCaption = data.InternalCaption;

        var newTranslations = from x in data.Translations
            where x.Id == default(int)
            let translation = (new Translation()).UpdateFromTranslationPrivateEditData(x)
            select translation;

        var updatedOldData = from x in data.Translations
            where x.Id != default(int)
            select x;

        var translationsAndChanges = from translation in content.Translations
            join changes in updatedOldData on translation.Id equals changes.Id
            select new { translation, changes };

        // Update existing translations
        foreach(var translationAndChange in translationsAndChanges)
        {
            translationAndChange.translation.UpdateFromTranslationPrivateEditData(translationAndChange.changes);
            translationAndChange.translation.UpdatedAt = updateTime;
        }

        // Add new translations
        foreach(var newTranslation in newTranslations)
        {
            newTranslation.UpdatedAt = updateTime;
            content.Translations.Add(newTranslation);
        }

        _context.Content.Update(content);
        await _context.SaveChangesAsync();

        return data;
    }

    /// <summary>
    /// Delete a content by id.
    /// </summary>
    /// <param name="contentId">Id of a content to delete.</param>
    public async Task DeleteAsync(int contentId)
    {
        var content = await _context.Content.SingleAsync(x => x.Id == contentId);

        _context.Content.Remove(content);
        await _context.SaveChangesAsync();
    }
}