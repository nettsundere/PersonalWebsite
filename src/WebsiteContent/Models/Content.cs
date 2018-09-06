using System.Collections.Generic;

namespace WebsiteContent.Models
{
    /// <summary>
    /// The content.
    /// </summary>
    public class Content
    {
        /// <summary>
        /// Content unique id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Available content translations.
        /// </summary>
        public IList<Translation> Translations { get; set; }

        /// <summary>
        /// Content non-translated caption.
        /// </summary>
        public string InternalCaption { get; set; }
    }
}
