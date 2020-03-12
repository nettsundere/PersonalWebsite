using System;
using System.Collections.Generic;
using WebsiteContent.Models;
using System.Linq;

namespace WebsiteContent.Repositories.DTO
{
    public class ContentPrivateEditData
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Internal caption.
        /// </summary>
        public string InternalCaption { get; set; }

        /// <summary>
        /// Translations.
        /// </summary>
        public IList<TranslationPrivateEditData> Translations { get; set; }

        /// <summary>
        /// Create <see cref="ContentPrivateEditData"/>
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="internalCaption">Internal caption.</param>
        /// <param name="translations">Content translations.</param>
        public ContentPrivateEditData(int id, string internalCaption, IList<TranslationPrivateEditData> translations)
        {
            Id = id;
            InternalCaption = internalCaption ?? throw new ArgumentNullException(nameof(internalCaption));
            Translations = translations ?? throw new ArgumentNullException(nameof(translations));
        }

        /// <summary>
        /// Create <see cref="ContentPrivateEditData"/>
        /// </summary>
        /// <param name="content">content</param>
        /// <exception cref="ArgumentNullException">The content is null.</exception>
        public ContentPrivateEditData(Content content)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            Id = content.Id;
            InternalCaption = content.InternalCaption;
            Translations = content.Translations.Select(x => new TranslationPrivateEditData(x)).ToList();
        }
    }
}
