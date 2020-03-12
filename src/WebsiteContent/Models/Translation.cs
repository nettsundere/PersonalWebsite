using System;
using System.ComponentModel.DataAnnotations;
using WebsiteContent.Lib;

namespace WebsiteContent.Models
{
    /// <summary>
    /// The translation of a content.
    /// </summary>
    public class Translation
    {
        /// <summary>
        /// Translation unique id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The language of a translation.
        /// </summary>
        public LanguageDefinition Version { get; set; }

        /// <summary>
        /// The <see cref="DataAvailabilityState"/> of a translation.
        /// </summary>
        public DataAvailabilityState State { get; set; }

        /// <summary>
        /// Name for the url string.
        /// </summary>
        [Required]
        public string UrlName { get; set; } = null!;

        /// <summary>
        /// Content markup.
        /// </summary>
        [Required]
        public string ContentMarkup { get; set; } = null!;

        /// <summary>
        /// Custom Header markup.
        /// </summary>
        public string? CustomHeaderMarkup { get; set; }

        /// <summary>
        /// Content title.
        /// </summary>
        [Required]
        public string Title { get; set; } = null!;

        /// <summary>
        /// Content description.
        /// </summary>
        [Required]
        public string Description { get; set; } = null!;

        /// <summary>
        /// Update timestamp.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// The reference to the content.
        /// </summary>
        public int ContentId { get; set; }

        /// <summary>
        /// Content.
        /// </summary>
        [Required]
        public virtual Content Content { get; set; } = null!;
    }
}
