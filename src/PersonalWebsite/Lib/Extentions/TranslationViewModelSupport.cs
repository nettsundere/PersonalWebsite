using PersonalWebsite.Models;
using PersonalWebsite.ViewModels.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsite.Lib.Extentions
{
    public static class TranslationViewModelSupport
    {
        public static Translation UpdateFromViewModel(this Translation translation, TranslationEditViewModel translationViewModel)
        {
            if (translationViewModel == null)
            {
                throw new ArgumentNullException(nameof(translationViewModel));
            }

            translation.ContentMarkup = translationViewModel.ContentMarkup;
            translation.ContentId = translationViewModel.ContentId;
            translation.Description = translationViewModel.Description;
            translation.State = translationViewModel.State;
            translation.Title = translationViewModel.Title;
            translation.UpdatedAt = translationViewModel.UpdatedAt;
            translation.UrlName = translationViewModel.UrlName;
            translation.Version = translationViewModel.Version;

            return translation;
        }
    }
}
