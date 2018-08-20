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
        public string UrlName { get; set; }

        /// <summary>
        /// Link content text.
        /// </summary>
        public string LinkTitle { get; set; }
    }
}
