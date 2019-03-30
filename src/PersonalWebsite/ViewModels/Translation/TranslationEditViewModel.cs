using System;
using System.ComponentModel.DataAnnotations;
using WebsiteContent.Lib;
using WebsiteContent.Repositories.DTO;

namespace PersonalWebsite.ViewModels.Translation
{
    /// <summary>
    /// Translation edit view model.
    /// </summary>
    public class TranslationEditViewModel : TranslationPrivateEditData
    {
        [Required]
        public override string Title { get; set; }

        [Required]
        public override LanguageDefinition Version { get; set; }

        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public override string Description { get; set; }

        /// <summary>
        /// Custom Header markup.
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public override string CustomHeaderMarkup { get; set; }

        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public override string ContentMarkup { get; set; }

        [Required]
        public override DataAvailabilityState State { get; set; }

        [Required]
        public override string UrlName { get; set; }

        [Required]
        public override DateTime UpdatedAt { get; set; }

        [Required]
        public override int ContentId { get; set; }

        /// <summary>
        /// Create <see cref="TranslationEditViewModel"/>.
        /// </summary>
        public TranslationEditViewModel() : this(new TranslationPrivateEditData())
        {
        }

        /// <summary>
        /// Create <see cref="TranslationEditViewModel"/>.
        /// </summary>
        /// <param name="data">Translation edit data.</param>
        public TranslationEditViewModel(TranslationPrivateEditData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            ContentId = data.ContentId;
            CustomHeaderMarkup = data.CustomHeaderMarkup;
            ContentMarkup = data.ContentMarkup;
            Description = data.Description;
            Title = data.Title;
            Id = data.Id;
            State = data.State;
            UpdatedAt = data.UpdatedAt;
            UrlName = data.UrlName;
            Version = data.Version;
        }
    }
}
