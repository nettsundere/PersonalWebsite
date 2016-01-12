using System.Collections.Generic;

namespace PersonalWebsite.Models
{
    public class Content
    {
        /// <summary>
        /// Content unique id.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Available content translations.
        /// </summary>
        public virtual IList<Translation> Translations { get; set; }

        /// <summary>
        /// Content non-translated caption.
        /// </summary>
        public virtual string InternalCaption { get; set; }
    }
}
