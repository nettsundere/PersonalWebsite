using PersonalWebsite.Lib;
using System.Collections.Generic;

namespace PersonalWebsite.ViewModels.Content
{
    /// <summary>
    /// Content view model.
    /// </summary>
    public class ContentViewModel
    {
        /// <summary>
        /// Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Html markup.
        /// </summary>
        public string Markup { get; set; }

        /// <summary>
        /// Page description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Internal caption.
        /// </summary>
        public string InternalCaption { get; set; }

        /// <summary>
        /// Url names (different translations).
        /// </summary>
        public IDictionary<LanguageDefinition, string> UrlNames { get; set; }
    }
}
