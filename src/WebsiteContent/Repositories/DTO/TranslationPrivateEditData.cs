using System;
using WebsiteContent.Lib;
using WebsiteContent.Models;

namespace WebsiteContent.Repositories.DTO
{
    public class TranslationPrivateEditData
    {
        public int Id { get; set; }

        public virtual string Title { get; set; }

        public virtual LanguageDefinition Version { get; set; }

        public virtual string Description { get; set; }

        public virtual string ContentMarkup { get; set; }

        public virtual DataAvailabilityState State { get; set; }

        public virtual string UrlName { get; set; }

        public virtual DateTime UpdatedAt { get; set; }

        public virtual int ContentId { get; set; }

        public TranslationPrivateEditData()
        {
        }

        public TranslationPrivateEditData(Translation translation)
        {
            if (translation == null)
            {
                throw new ArgumentNullException(nameof(translation));
            }

            Id = translation.Id;
            Title = translation.Title;
            Version = translation.Version;
            Description = translation.Description;
            ContentMarkup = translation.ContentMarkup;
            State = translation.State;
            UrlName = translation.UrlName;
            UpdatedAt = translation.UpdatedAt;
            ContentId = translation.ContentId;
        }
    }
}
