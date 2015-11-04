using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsite.Models
{
    public class Content
    {
        /// <summary>
        /// Content unique identifier.
        /// </summary>
        public virtual Guid ContentGuid { get; set; }

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
