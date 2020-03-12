using System;
using WebsiteContent.Models;
using WebsiteContent.Repositories.DTO;

namespace PersonalWebsite.Lib.Extensions
{
    public static class TranslationPrivateEditDataSupport
    {
        public static Translation UpdateFromTranslationPrivateEditData(this Translation translation, TranslationPrivateEditData translationEditData)
        {
            if (translation is null)
            {
                throw new ArgumentNullException(nameof(translation));
            }
            
            if (translationEditData is null)
            {
                throw new ArgumentNullException(nameof(translationEditData));
            }

            translation.CustomHeaderMarkup = translationEditData.CustomHeaderMarkup ?? throw new InvalidOperationException(nameof(translationEditData.CustomHeaderMarkup));
            translation.ContentMarkup = translationEditData.ContentMarkup ?? throw new InvalidOperationException(nameof(translationEditData.ContentMarkup));
            translation.ContentId = translationEditData.ContentId;
            translation.Description = translationEditData.Description ?? throw new InvalidOperationException(nameof(translationEditData.Description));
            translation.State = translationEditData.State;
            translation.Title = translationEditData.Title ?? throw new InvalidOperationException(nameof(translationEditData.Title));
            translation.UpdatedAt = translationEditData.UpdatedAt;
            translation.UrlName = translationEditData.UrlName ?? throw new InvalidOperationException(nameof(translationEditData.UrlName));
            translation.Version = translationEditData.Version;

            return translation;
        }
    }
}
