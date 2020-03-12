using System;
using WebsiteContent.Lib;
using WebsiteContent.Models;

namespace WebsiteContent.Repositories.DTO
{
    public class TranslationPrivateEditData
    {
        public int Id { get; set; }

        public virtual string? Title { get; set; }

        public virtual LanguageDefinition Version { get; set; }

        public virtual string? Description { get; set; }

        /// <summary>
        /// Custom Header Markup.
        /// </summary>
        public virtual string? CustomHeaderMarkup { get; set; }
        
        /// <summary>
        /// Content markup.
        /// </summary>
        public virtual string? ContentMarkup { get; set; }

        /// <summary>
        /// Data availability state.
        /// </summary>
        public virtual DataAvailabilityState State { get; set; }

        /// <summary>
        /// The name of a content in URL.
        /// </summary>
        public virtual string? UrlName { get; set; }

        /// <summary>
        /// Update timestamp.
        /// </summary>
        public virtual DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Content identifier.
        /// </summary>
        public virtual int ContentId { get; set; }

        public TranslationPrivateEditData()
        {
        }

        /// <summary>
        /// Create <see cref="TranslationPrivateEditData"/>
        /// </summary>
        /// <param name="translation">Translation to create <see cref="TranslationPrivateEditData"/> from.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public TranslationPrivateEditData(Translation translation)
        {
            if (translation is null)
            {
                throw new ArgumentNullException(nameof(translation));
            }

            Id = translation.Id;
            Title = translation.Title;
            Version = translation.Version;
            Description = translation.Description;
            CustomHeaderMarkup = translation.CustomHeaderMarkup;
            ContentMarkup = translation.ContentMarkup;
            State = translation.State;
            UrlName = translation.UrlName;
            UpdatedAt = translation.UpdatedAt;
            ContentId = translation.ContentId;
        }
    }
}
