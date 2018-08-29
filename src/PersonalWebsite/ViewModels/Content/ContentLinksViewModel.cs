using System;
using WebsiteContent.Repositories.DTO;

namespace PersonalWebsite.ViewModels.Content
{
    /// <summary>
    /// Represents data required to display all human-readable links to content pages depending on current language. 
    /// </summary>
    public class ContentLinksViewModel : ContentPublicLinksData
    {
        /// <summary>
        /// Create <see cref="ContentLinksViewModel"/>.
        /// </summary>
        /// <param name="data">Public content links data.</param>
        public ContentLinksViewModel(ContentPublicLinksData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            InternalNamesToLinks = data.InternalNamesToLinks;
        }

        /// <summary>
        /// Get a caption for a page having <paramref name="internalContentCaption"/>.
        /// </summary>
        /// <param name="internalContentCaption">Internal page caption.</param>
        /// <returns>Caption for a page having <paramref name="internalContentCaption"/>.</returns>
        public string CaptionFor(string internalContentCaption)
        {
            if (internalContentCaption == null)
            {
                throw new ArgumentNullException(nameof(internalContentCaption));
            }

            return InternalNamesToLinks[internalContentCaption].LinkTitle;
        }

        /// <summary>
        /// Get a url name for a page having <paramref name="internalContentCaption"/>
        /// </summary>
        /// <param name="internalContentCaption">Internal page caption.</param>
        /// <returns>Url name for a page having <paramref name="internalContentCaption"/>.</returns>
        public string UrlNameFor(string internalContentCaption)
        {
            if (internalContentCaption == null)
            {
                throw new ArgumentNullException(nameof(internalContentCaption));
            }

            return InternalNamesToLinks[internalContentCaption].UrlName;
        }
    }
} 
