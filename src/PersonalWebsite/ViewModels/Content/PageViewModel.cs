using PersonalWebsite.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsite.ViewModels.Content
{
    /// <summary>
    /// Represents view model required to render a page.
    /// </summary>
    public class PageViewModel
    {
        public LanguageDefinition Language { get; private set; }
        public ContentViewModel ContentViewModel { get; private set; }
        public ContentLinksViewModel ContentLinksViewModel { get; private set; }

        public PageViewModel(LanguageDefinition language, ContentViewModel contentViewModel, ContentLinksViewModel contentLinksViewModel)
        {
            if (contentViewModel == null)
            {
                throw new ArgumentNullException(nameof(contentViewModel));
            }

            if(contentLinksViewModel == null)
            {
                throw new ArgumentNullException(nameof(contentLinksViewModel));
            }

            Language = language;
            ContentViewModel = contentViewModel;
            ContentLinksViewModel = contentLinksViewModel;
        }
    }
}
