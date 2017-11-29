using System;
using System.Collections.Generic;
using System.Text;
using WebsiteContent.Lib;

namespace WebsiteContent.Repositories.DTO
{
    public class ContentPublicLinksData
    {
        public IDictionary<string, ContentPublicLinkUI> InternalNamesToLinks { get; set; }
    }
}
