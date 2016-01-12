using PersonalWebsite.Lib;
using System;

namespace PersonalWebsite.Models
{
    public class Translation
    {
        public int Id { get; set; }
        public LanguageDefinition Version { get; set; }
        public DataAvailabilityState State { get; set; }

        /// <summary>
        /// Name for the url string.
        /// </summary>
        public string UrlName { get; set; }

        public string ContentMarkup { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int ContentId { get; set; }
        public virtual Content Content { get; set; }
    }
}
