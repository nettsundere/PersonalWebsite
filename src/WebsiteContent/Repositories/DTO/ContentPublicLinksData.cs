using System.Collections.Generic;

namespace WebsiteContent.Repositories.DTO
{
    public class ContentPublicLinksData
    {
        public IDictionary<string, ContentPublicLinkUI> InternalNamesToLinks { get; set; }
    }
}
