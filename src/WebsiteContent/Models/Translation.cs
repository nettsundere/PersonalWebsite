using System;
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
        public string UrlName { get; set; }

        /// <summary>
        /// Content markup.
        /// </summary>
        public string ContentMarkup { get; set; }

        /// <summary>
        /// Content title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Content description.
        /// </summary>
        public string Description { get; set; }

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
        public virtual Content Content { get; set; }
    }
}
