using PersonalWebsite.Lib;
using System.Collections.Generic;

namespace PersonalWebsite.ViewModels.Content
{
    public class ContentViewModel
    {
        public string Title { get; set; }
        public string Markup { get; set; }
        public string Description { get; set; }
        public string InternalCaption { get; set; }
        public IDictionary<LanguageDefinition, string> UrlNames { get; set; }
    }
}
