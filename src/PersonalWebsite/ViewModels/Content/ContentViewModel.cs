using PersonalWebsite.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsite.ViewModels.Content
{
    public class ContentViewModel
    {
        public string Title { get; set; }
        public string Markup { get; set; }
        public string Description { get; set; }
        public IDictionary<LanguageDefinition, string> UrlNames { get; set; }
    }
}
