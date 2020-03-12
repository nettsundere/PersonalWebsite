using System;
using System.Collections.Generic;

namespace WebsiteContent.Repositories.DTO
{
    public class ContentPublicLinksData
    {
        public IDictionary<string, ContentPublicLinkUI> InternalNamesToLinks { get; }

        public ContentPublicLinksData(IDictionary<string, ContentPublicLinkUI> internalNamesToLinks)
        {
            InternalNamesToLinks =
                internalNamesToLinks ?? throw new ArgumentNullException(nameof(internalNamesToLinks));
        } 
    }
}
