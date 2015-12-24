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
        public ContentViewModel ContentViewModel { get; private set; }
        public ContentLinksViewModel ContentLinksViewModel { get; private set; }

        public PageViewModel(ContentViewModel contentViewModel, ContentLinksViewModel contentLinksViewModel)
        {
            if (contentViewModel == null)
            {
                throw new ArgumentNullException(nameof(contentViewModel));
            }

            if(contentLinksViewModel == null)
            {
                throw new ArgumentNullException(nameof(contentLinksViewModel));
            }

            ContentViewModel = contentViewModel;
            ContentLinksViewModel = contentLinksViewModel;
        }
    }
}
