using PersonalWebsite.ViewModels.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersonalWebsite.Models;
using System.ComponentModel.DataAnnotations;

namespace PersonalWebsite.ViewModels.Content
{
    public class ContentEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public string InternalCaption { get; set; }

        public IList<TranslationEditViewModel> Translations { get; set; }

        public ContentEditViewModel()
        {
        }

        public ContentEditViewModel(Models.Content content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            Id = content.Id;
            InternalCaption = content.InternalCaption;

            if(content.Translations != null)
            {
                Translations = (from x in content.Translations
                                select new TranslationEditViewModel(x)).ToList();
            }
        }
    }
}
