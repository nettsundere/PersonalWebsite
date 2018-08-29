using Microsoft.EntityFrameworkCore;
using PersonalWebsite.Lib.Extentions;
using PersonalWebsite.Models;
using System;
using System.Linq;
using WebsiteContent.Lib;
using WebsiteContent.Models;
using WebsiteContent.Repositories;
using WebsiteContent.Repositories.DTO;

namespace PersonalWebsite.Repositories
{
    /// <summary>
    /// Content editor repository.
    /// </summary>
    public class ContentEditorRepository : IContentEditorRepository
    {
        /// <summary>
        /// Disposing status.
        /// </summary>
        private bool _isDisposed;

        /// <summary>
        /// Data context.
        /// </summary>
        private readonly DataDbContext _dataDbContext;

        /// <summary>
        /// Create <see cref="ContentEditorRepository"/>.
        /// </summary>
        /// <param name="dataDbContext">Data context.</param>
        public ContentEditorRepository(DataDbContext dataDbContext)
        {
            _dataDbContext = dataDbContext ?? throw new ArgumentNullException(nameof(dataDbContext));
        }

        /// <summary>
        /// Create <see cref="ContentPrivateEditData"/> representation in DB.
        /// </summary>
        /// <param name="contentEditViewModel">Content representation to store in DB.</param>
        /// <returns>Updated content representation.</returns>
        public ContentPrivateEditData Create(ContentPrivateEditData contentEditViewModel)
        {
            GuardNotDisposed();

            var content = _dataDbContext.Content.Add(new Content
            {
                InternalCaption = contentEditViewModel.InternalCaption
            });

            _dataDbContext.SaveChanges();

            var entity = content.Entity;

            return new ContentPrivateEditData(entity);
        }

        /// <summary>
        /// Read a content.
        /// </summary>
        /// <param name="contentId">Id of a content to read.</param>
        /// <returns>Content representation.</returns>
        public ContentPrivateEditData Read(int contentId)
        {
            GuardNotDisposed();

            var content = _dataDbContext.Content.Include(c => c.Translations).FirstOrDefault(x => x.Id == contentId);
            if (content != null)
            {
                var data = new ContentPrivateEditData(content);

                // Build missing translations
                var missingTranslations = from x in Enum.GetValues(typeof(LanguageDefinition)).Cast<LanguageDefinition>()
                                            where !data.Translations.Select(t => t.Version).Contains(x)
                                            select new TranslationPrivateEditData(new Translation()) { ContentId = data.Id, Version = x };

                data.Translations = data.Translations.Concat(missingTranslations).ToList();

                return data;
            }
            else
            {
                return null;
            }       
        }

        /// <summary>
        /// Read content list.
        /// </summary>
        /// <returns>List of content representations.</returns>
        public ContentPrivateEditListData ReadList()
        {
            GuardNotDisposed();

            var contentsQuery = from x in _dataDbContext.Content
                               orderby x.Id
                               select new ContentPrivateLinksData
                               {
                                   Id = x.Id,
                                   InternalCaption = x.InternalCaption
                               };

            return new ContentPrivateEditListData
            {
                Contents = contentsQuery.ToList()
            };
        }

        /// <summary>
        /// Update a content.
        /// </summary>
        /// <param name="data">Data to update with.</param>
        /// <returns>Updated content.</returns>
        public ContentPrivateEditData Update(ContentPrivateEditData data)
        {
            GuardNotDisposed();

            var content = _dataDbContext.Content.Include(c => c.Translations)
                                        .Single(x => x.Id == data.Id);

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

            _dataDbContext.Content.Update(content);
            _dataDbContext.SaveChanges();

            return data;
        }

        /// <summary>
        /// Delete a content by id.
        /// </summary>
        /// <param name="contentId">Id of a content to delete.</param>
        public void Delete(int contentId)
        {
            GuardNotDisposed();
            var content = _dataDbContext.Content.Single(x => x.Id == contentId);

            _dataDbContext.Content.Remove(content);
            _dataDbContext.SaveChanges();
        }

        /// <summary>
        /// Dispose the object.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _dataDbContext.Dispose();
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~ContentEditorRepository()
        {
            Dispose();
        }

        /// <summary>
        /// Throw if <see cref="ContentEditorRepository"/> is disposed.
        /// </summary>
        private void GuardNotDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(ContentEditorRepository));
            }
        }
    }
}
