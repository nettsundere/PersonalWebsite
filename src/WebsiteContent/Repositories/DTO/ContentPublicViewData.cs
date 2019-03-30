using System.Collections.Generic;
using WebsiteContent.Lib;

namespace WebsiteContent.Repositories.DTO
{
    public class ContentPublicViewData
    {
        /// <summary>
        /// Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Custom Header Markup.
        /// </summary>
        public string CustomHeaderMarkup { get; set; }
        
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
