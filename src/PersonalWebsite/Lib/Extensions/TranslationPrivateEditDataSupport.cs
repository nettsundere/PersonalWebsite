using System;
using WebsiteContent.Models;
using WebsiteContent.Repositories.DTO;

namespace PersonalWebsite.Lib.Extensions
{
    public static class TranslationPrivateEditDataSupport
    {
        public static Translation UpdateFromTranslationPrivateEditData(this Translation translation, TranslationPrivateEditData translationEditData)
        {
            if (translation == null)
            {
                throw new ArgumentNullException(nameof(translation));
            }
            
            if (translationEditData == null)
            {
                throw new ArgumentNullException(nameof(translationEditData));
            }

            translation.CustomHeaderMarkup = translationEditData.CustomHeaderMarkup;
            translation.ContentMarkup = translationEditData.ContentMarkup;
            translation.ContentId = translationEditData.ContentId;
            translation.Description = translationEditData.Description;
            translation.State = translationEditData.State;
            translation.Title = translationEditData.Title;
            translation.UpdatedAt = translationEditData.UpdatedAt;
            translation.UrlName = translationEditData.UrlName;
            translation.Version = translationEditData.Version;

            return translation;
        }
    }
}
