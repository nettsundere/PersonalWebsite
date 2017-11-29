using System;
using System.Collections.Generic;
using WebsiteContent.Models;
using System.Linq;

namespace WebsiteContent.Repositories.DTO
{
    public class ContentPrivateEditData
    {
        public int Id { get; set; }

        public virtual string InternalCaption { get; set; }

        public virtual IList<TranslationPrivateEditData> Translations { get; set; }

        public ContentPrivateEditData()
        {
        }

        public ContentPrivateEditData(Content content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            Id = content.Id;
            InternalCaption = content.InternalCaption;
            Translations = content.Translations?.Select(x => new TranslationPrivateEditData(x)).ToList();
        }
    }
}
