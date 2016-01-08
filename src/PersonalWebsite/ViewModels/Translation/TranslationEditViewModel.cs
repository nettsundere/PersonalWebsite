using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersonalWebsite.Models;
using PersonalWebsite.Lib;
using System.ComponentModel.DataAnnotations;

namespace PersonalWebsite.ViewModels.Translation
{
    public class TranslationEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public LanguageDefinition Version { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Description { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ContentMarkup { get; set; }

        [Required]
        public DataAvailabilityState State { get; set; }

        [Required]
        public string UrlName { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int ContentId { get; set; }

        public TranslationEditViewModel()
        {
        }

        public TranslationEditViewModel(Models.Translation translation)
        {
            if(translation == null)
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
