using System;

namespace WebsiteContent.Repositories.DTO
{
    /// <summary>
    /// Presents url and content for human readable link.
    /// </summary>
    public class ContentPublicLinkUI
    {
        /// <summary>
        /// Link URL human-readable content.
        /// </summary>
        public string UrlName { get; }

        /// <summary>
        /// Link content text.
        /// </summary>
        public string LinkTitle { get; }

        public ContentPublicLinkUI(string urlName, string linkTitle)
        {
            UrlName = urlName ?? throw new ArgumentNullException(nameof(urlName));
            LinkTitle = linkTitle ?? throw new ArgumentNullException(nameof(linkTitle));
        }
    }
}
