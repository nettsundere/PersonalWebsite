using PersonalWebsite.ViewModels.Translation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebsiteContent.Repositories.DTO;

namespace PersonalWebsite.ViewModels.Content
{
    /// <summary>
    /// Content and translation edit view model.
    /// </summary>
    public class ContentAndTranslationsEditViewModel
    {
        /// <summary>
        /// Content id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// An internal caption of a content.
        /// </summary>
        [Required]
        public string InternalCaption { get; set; } = null!;

        /// <summary>
        /// Available translations of a content.
        /// </summary>
        [Required]
        public IList<TranslationEditViewModel> Translations { get; set; } = null!;

        /// <summary>
        /// Create <see cref="ContentAndTranslationsEditViewModel"/>.
        /// </summary>
        public ContentAndTranslationsEditViewModel()
        {
        }
        
        /// <summary>
        /// Create <see cref="ContentAndTranslationsEditViewModel"/>.
        /// </summary>
        /// <param name="contentPrivateEditData">The data required to edit a content.</param>
        public ContentAndTranslationsEditViewModel(ContentPrivateEditData contentPrivateEditData)
        {
            if (contentPrivateEditData is null)
            {
                throw new ArgumentNullException(nameof(contentPrivateEditData));
            }

            Id = contentPrivateEditData.Id;
            InternalCaption = contentPrivateEditData.InternalCaption;
            Translations = contentPrivateEditData.Translations.Select(x => new TranslationEditViewModel(x)).ToList();
        }

        /// <summary>
        /// Get the data from this view model.
        /// </summary>
        /// <returns>The data from this view model.</returns>
        public ContentPrivateEditData GetContentEditData() => new ContentPrivateEditData(Id, InternalCaption,
            Translations.ToList<TranslationPrivateEditData>());
    }
}
